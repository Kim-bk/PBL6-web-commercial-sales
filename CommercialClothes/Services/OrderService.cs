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
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IItemRepository _itemRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IUserRepository _userRepo;
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork
                    , IMapperCustom mapper,IOrderDetailRepository orderDetailRepository
                    , IItemRepository itemRepository, IImageRepository imageRepository
                    , IUserRepository userRepo) : base(unitOfWork, mapper)


        {
            _imageRepo = imageRepository;
            _orderRepo = orderRepository;
            _orderDetailRepo = orderDetailRepository;
            _itemRepo = itemRepository;
            _userRepo = userRepo;
        }

        public async Task<string> AddOrder(OrderRequest req, int idAccount)
        {
            string cartId = "";
            try
            {
                await _unitOfWork.BeginTransaction();
                var findOrder = await _orderRepo.GetCart(idAccount);
                var user = await _userRepo.FindAsync(us => us.Id == idAccount);
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
                        _orderRepo.Update(item);
                        var findOrderDetail = await _orderDetailRepo.ListOrderDetail(item.Id);
                        foreach (var lord in findOrderDetail)
                        {
                            var finditem = await _itemRepo.GetItemById(lord.ItemId);
                            foreach (var itemu in finditem)
                            {
                                itemu.Quantity = itemu.Quantity + lord.Quantity.Value;
                                _itemRepo.Update(itemu);
                            }
                            lord.Price = lord.Quantity.Value * lord.Item.Price;
                            _orderDetailRepo.Update(lord);
                        }
                    }
                    await _unitOfWork.CommitTransaction();     
                }

                foreach (var item in req.Details)
                {
                    var order = new Order
                    {
                        Account = user,
                        DateCreate = DateTime.Now,
                        IsBought = true,
                        Address = req.Address,
                        PaymentId = req.PaymentId,
                        StatusId = 1,
                        PhoneNumber = req.PhoneNumber,
                        ShopId = item.ShopId,
                    };

                    await _orderRepo.AddAsync(order);
                    await _unitOfWork.CommitTransaction();

                    // get cart Id
                    cartId += order.Id.ToString();

                    foreach (var ord in item.OrderDetails)
                    {
                        var findItem = await _itemRepo.FindAsync(it => it.Id == ord.ItemId);
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
            var findOrder = await _orderRepo.FindAsync(or => or.Id == orderId);
            if (findOrder == null)
            {
                return false;
            }
            await _unitOfWork.BeginTransaction();
            findOrder.StatusId = 4;
            var findOrderDetail = await _orderDetailRepo.ListOrderDetail(findOrder.Id);
            foreach (var lord in findOrderDetail)
            {
                var finditem = await _itemRepo.GetItemById(lord.ItemId);
                foreach (var item in finditem)
                {
                    item.Quantity = item.Quantity - lord.Quantity.Value;
                    _itemRepo.Update(item);
                }
            }
            _orderRepo.Update(findOrder);
            await _unitOfWork.CommitTransaction();
            return true;
        }
        public async Task<OrderResponse> GetOrderDetails(int orderId)
        {
            var findOrder = await _orderRepo.FindAsync(or => or.Id == orderId);
            var orderDetails = await _orderDetailRepo.ListOrderDetail(findOrder.Id);
            var getOrder = _orderRepo.GetOrders(findOrder.Id);
            if (orderDetails == null)
            {   
                return new OrderResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Không tìm thấy chi tiết đơn hàng!",
                };
            }
            var ordDetail = new List<OrderDetailDTO>();
            var lorder = new List<OrderDTO>();
            foreach (var item in orderDetails)
            {
                var imgItem = await _imageRepo.GetImage(item.Item.Id);
                var orderDetail = new OrderDetailDTO()
                {
                    Id = item.Id,
                    Quantity = item.Quantity.Value,
                    ItemName = item.Item.Name,
                    Size = item.Item.Size,
                    ItemId = item.Item.Id,
                    ItemImg = imgItem.Path,
                    Price = item.Item.Price * item.Quantity.Value
                };
                ordDetail.Add(orderDetail);
            }    
            var ord = new OrderDTO()
            {
                Id = findOrder.Id,
                StatusId = findOrder.StatusId.Value,
                StatusName = findOrder.Status.Name,
                PaymentName = findOrder.Payment.Type,
                DateCreated = findOrder.DateCreate,
                PhoneNumber = findOrder.PhoneNumber,
                NameOrder =  findOrder.Account.Name,
                Address = findOrder.Address,
                OrderDetails = ordDetail
            };
            lorder.Add(ord);
            var ordRes = new OrderResponse()
            {
                IsSuccess = true,
                Orders = lorder,
            };
            return ordRes;
        }

        public async Task<StatusResponse> UpdateStatusOrder(StatusRequest req,int orderId)
        {
            try
            {
                var findOrder = await _orderRepo.FindAsync(or => or.Id == orderId);
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
                    _orderRepo.Update(findOrder);
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