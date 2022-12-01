using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly IItemRepository _itemRepository;
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork
                    , IMapperCustom mapper,IOrderDetailRepository orderDetailRepository
                    , IItemRepository itemRepository) : base(unitOfWork, mapper)

        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
            _itemRepository = itemRepository;
        }

        public async Task<string> AddOrder(OrderRequest req, int idAccount)
        {
            string cartId = "";
            try
            {
                await _unitOfWork.BeginTransaction();
                var findOrder = await _orderRepository.GetCart(idAccount);
                if(findOrder.Count != 0)
                {
                    foreach (var item in findOrder)
                    {
                        cartId += item.Id.ToString();
                        item.IsBought = true;
                        item.Address = req.Address;
                        item.PaymentId = req.PaymentId;
                        item.DateCreate = DateTime.Now;
                        item.StatusId = 1;
                        item.PhoneNumber = req.PhoneNumber;
                        _orderRepository.Update(item);
                        var findOrderDetail = await _orderDetailRepository.ListOrderDetail(item.Id);
                        foreach (var lord in findOrderDetail)
                        {
                            var finditem = await _itemRepository.GetItemById(lord.ItemId);
                            foreach (var itemu in finditem)
                            {
                                itemu.Quantity = itemu.Quantity + lord.Quantity.Value;
                                _itemRepository.Update(itemu);
                            }
                            lord.Price = lord.Quantity.Value * lord.Item.Price;
                            _orderDetailRepository.Update(lord);
                        }
                    }
                    await _unitOfWork.CommitTransaction();     
                }

                foreach (var item in req.Details)
                {
                    var order = new Order
                    {
                        AccountId = idAccount,
                        DateCreate = DateTime.UtcNow,
                        IsBought = true,
                        Address = req.Address,
                        PaymentId = req.PaymentId,
                        StatusId = 1,
                        PhoneNumber = req.PhoneNumber,
                        ShopId = item.ShopId,
                    };

                    await _orderRepository.AddAsync(order);
                    await _unitOfWork.CommitTransaction();

                    // get cart Id
                    cartId += order.Id.ToString();

                    foreach (var ord in item.OrderDetails)
                    {
                        var findItem = await _itemRepository.FindAsync(it => it.Id == ord.ItemId);
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ItemId = ord.ItemId,
                            Quantity = ord.Quantity,
                            Price = findItem.Price * ord.Quantity
                        };
                        order.OrderDetails.Add(orderDetail);
                    }
                }

                await _unitOfWork.CommitTransaction();
                // return id of this cart by match every orderId together
                return "#ORD" + cartId;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<bool> CancelOrder(int orderId)
        {
            var findOrder = await _orderRepository.FindAsync(or => or.Id == orderId);
            if (findOrder == null)
            {
                return false;
            }
            await _unitOfWork.BeginTransaction();
            findOrder.StatusId = 4;
            var findOrderDetail = await _orderDetailRepository.ListOrderDetail(findOrder.Id);
            foreach (var lord in findOrderDetail)
            {
                var finditem = await _itemRepository.GetItemById(lord.ItemId);
                foreach (var item in finditem)
                {
                    item.Quantity = item.Quantity - lord.Quantity.Value;
                    _itemRepository.Update(item);
                }
            }
            _orderRepository.Update(findOrder);
            await _unitOfWork.CommitTransaction();
            return true;
        }

        public async Task<OrderDetailResponse> GetOrderDetails(int orderId)
        {
            var orderDetails = await _orderDetailRepository.ListOrderDetail(orderId);
            if (orderDetails == null)
            {
                return new OrderDetailResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Không tìm thấy Order Detail !",
                };
            }    

            return new OrderDetailResponse
            {
                IsSuccess = true,
                OrderDetail = _mapper.MapOrderDetails(orderDetails)
            };
        }

        public async Task<StatusResponse> UpdateStatusOrder(StatusRequest req,int orderId)
        {
            try
            {
                var findOrder = await _orderRepository.FindAsync(or => or.Id == orderId);
                if (findOrder == null)
                {
                    return new StatusResponse{
                        IsSuccess = false,
                        ErrorMessage = "Order not found"
                    };
                }
                if (req.StatusId <= 3)
                {
                    await _unitOfWork.BeginTransaction();
                    findOrder.StatusId = req.StatusId;
                    _orderRepository.Update(findOrder);
                    await _unitOfWork.CommitTransaction();
                    return new StatusResponse
                    {
                        IsSuccess = true,
                    };
                }
                return new StatusResponse{
                    IsSuccess = false,
                    ErrorMessage = "Order was cancel"
                };

            }
            catch (Exception ex)
            {                
                ex = new Exception(ex.Message);
                throw ex;
            }
        }
        
    }
}