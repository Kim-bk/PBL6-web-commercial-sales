using System;
using System.Collections.Generic;
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
using Model.DTOs;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Ocsp;
using Stripe;

namespace CommercialClothes.Services
{
    public class OrderService : BaseService, IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IItemRepository _itemRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IUserRepository _userRepo;
        private readonly IAdminService _adminService;

        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork
                    , IMapperCustom mapper, IOrderDetailRepository orderDetailRepository
                    , IItemRepository itemRepository, IImageRepository imageRepository
                    , IUserRepository userRepo, IAdminService adminService) : base(unitOfWork, mapper)

        {
            _imageRepo = imageRepository;
            _orderRepo = orderRepository;
            _orderDetailRepo = orderDetailRepository;
            _itemRepo = itemRepository;
            _userRepo = userRepo;
            _adminService = adminService;
        }

        public async Task<string> AddOrder(OrderRequest req, int idAccount)
        {
            string cartId = "#2CLOTHYORD";
            string orderBillId = "#2CLOTHYORD";
            try
            {
                await _unitOfWork.BeginTransaction();
                var findOrder = await _orderRepo.GetCart(idAccount);
                var user = await _userRepo.FindAsync(us => us.Id == idAccount);
                if (findOrder.Count != 0)
                {
                    foreach (var item in findOrder)
                    {
                        cartId += "-" + item.Id.ToString();
                        orderBillId += item.Id.ToString();
                        item.BillId = cartId;
                        item.IsBought = true;
                        item.IsSuccess = false;
                        item.Address = req.Address;
                        item.PaymentId = req.PaymentId;
                        item.DateCreate = DateTime.Now;
                        item.StatusId = 1;
                        item.PhoneNumber = req.PhoneNumber;
                       // _orderRepo.Update(item);
                        var findOrderDetail = await _orderDetailRepo.ListOrderDetail(item.Id);
                        foreach (var lord in findOrderDetail)
                        {
                            var finditem = await _itemRepo.GetItemById(lord.ItemId);
                            finditem.Quantity = finditem.Quantity - lord.Quantity.Value;
                            lord.Price = lord.Quantity.Value * lord.Item.Price;
                            item.Total += lord.Price;
                        }

                        orderBillId = "#2CLOTHYORD";
                    }
                    await _unitOfWork.CommitTransaction();
                    return cartId;
                }

                var tmpTotal = 0;
                foreach (var item in req.Details)
                {
                    var order = new Order
                    {
                        Account = user,
                        DateCreate = DateTime.Now,
                        IsBought = true,
                        IsSuccess = false,
                        Address = req.Address,
                        PaymentId = req.PaymentId,
                        City = req.City,
                        Country = req.Country,
                        StatusId = 1,
                        PhoneNumber = req.PhoneNumber,
                        ShopId = item.ShopId,
                    };
                    await _orderRepo.AddAsync(order);
                    await _unitOfWork.CommitTransaction();


                    // get cart Id
                    cartId += "-" + order.Id.ToString();
                    orderBillId += order.Id.ToString();
                    order.BillId = orderBillId;

                    foreach (var ord in item.OrderDetails)
                    {
                        var findItem = await _itemRepo.FindAsync(it => it.Id == ord.ItemId);
                        var orderDetail = new OrderDetail
                        {
                            OrderId = order.Id,
                            ItemId = ord.ItemId.Value,
                            Quantity = ord.Quantity,
                            Price = findItem.Price * ord.Quantity.Value

                        };

                        findItem.Quantity = findItem.Quantity - orderDetail.Quantity.Value;
                        tmpTotal += orderDetail.Price;
                        order.OrderDetails.Add(orderDetail);
                        _orderRepo.Update(order);
                        _itemRepo.Update(findItem);

                       /* var it = new Item
                        {
                            Quantity = findItem.Quantity - orderDetail.Quantity.Value,
                        };*/
                    }
                    var orderUpdate = await _orderRepo.FindAsync(it => it.Id == order.Id);
                    orderUpdate.Total = tmpTotal;

                    orderBillId = "#2CLOTHYORD";
                }
                await _unitOfWork.CommitTransaction();
                // return id of this cart by match every orderId together
                return cartId;
            }
            catch
            {
                throw;
            }
        }

        public async Task<bool> CancelOrder(int orderId)
        {
            var findOrder = await _orderRepo.FindAsync(or => or.Id == orderId);
            if (findOrder == null)
                return false;

            await _unitOfWork.BeginTransaction();
            findOrder.StatusId = 4;
            var findOrderDetail = await _orderDetailRepo.ListOrderDetail(findOrder.Id);
            foreach (var lord in findOrderDetail)
            {
                var findItem = await _itemRepo.GetItemById(lord.ItemId);
                findItem.Quantity = findItem.Quantity + lord.Quantity.Value;
                _itemRepo.Update(findItem);
            }
           
            // Test local lỗi khi không coment đoạn code 144 -> 150, khi comment lại thì ko lỗi 
            // Admin return back money to customer wallet when the order was canceled
            var transactionDto = new TransactionDTO
            {
                BillId = findOrder.BillId,
                CustomerId = findOrder.AccountId,
                ShopId = findOrder.ShopId,
                Money = findOrder.Total.Value,
            };
            _ = await _adminService.ManageTransaction(transactionDto, 2);

            await _unitOfWork.CommitTransaction();
            return true;
        }

        public async Task<OrderResponse> GetOrderDetails(int orderId)
        {
            var findOrder = await _orderRepo.FindAsync(or => or.Id == orderId);
            var orderDetails = await _orderDetailRepo.ListOrderDetail(findOrder.Id);
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
                NameOrder = findOrder.Account.Name,
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

        public async Task<StatusResponse> UpdateStatusOrder(StatusRequest req, int orderId)
        {
            try
            {
                var findOrder = await _orderRepo.FindAsync(or => or.Id == orderId && or.IsSuccess == true);
                if (findOrder == null)
                {
                    return new StatusResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Order not found"
                    };
                }

                if (req.StatusId <= 3)
                {
                    await _unitOfWork.BeginTransaction();
                    findOrder.StatusId = req.StatusId;
                    //_orderRepo.Update(findOrder);
                    await _unitOfWork.CommitTransaction();

                    // if status = "Đã giao" which means statusId = 3 then call admin service to transfer money from admin to shop
                    if (req.StatusId == 3)
                    {
                        var transactionDto = new TransactionDTO
                        {
                            BillId = findOrder.BillId,
                            CustomerId = findOrder.AccountId,
                            ShopId = findOrder.ShopId,
                            Money = findOrder.Total.Value,
                        };
                        await _adminService.ManageTransaction(transactionDto, 3);
                    }

                    return new StatusResponse
                    {
                        IsSuccess = true,
                    };
                }

                return new StatusResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Order was cancel"
                };
            }
            catch
            {
                throw;
            }
        }
    }
}