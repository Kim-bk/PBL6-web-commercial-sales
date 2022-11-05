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
        public OrderService(IOrderRepository orderRepository, IUnitOfWork unitOfWork
            , IMapperCustom mapper, IOrderDetailRepository orderDetailRepository) : base(unitOfWork, mapper)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<bool> AddOrder(OrderRequest req,int idAccount)
        {
            try
            {
                var findOrder = await _orderRepository.FindAsync(or => or.AccountId == idAccount && or.IsBought == false);
                if(findOrder != null)
                {
                    await _unitOfWork.BeginTransaction();
                    findOrder.IsBought = true;
                    findOrder.Address = req.Address;
                    findOrder.PaymentId = req.PaymentId;
                    findOrder.StatusId = 1;
                    findOrder.PhoneNumber = req.PhoneNumber;
                    _orderRepository.Update(findOrder);
                    await _unitOfWork.CommitTransaction();
                    return true;
                }
                await _unitOfWork.BeginTransaction();
                var order = new Order
                {
                    AccountId = idAccount,
                    DateCreate = DateTime.UtcNow,
                    IsBought = true,
                    Address = req.Address,
                    PaymentId = req.PaymentId,
                    StatusId = 1,
                    PhoneNumber = req.PhoneNumber,
                };
                await _orderRepository.AddAsync(order);
                foreach (var ord in req.OrderDetails)
                {
                    var orderDetail = new OrderDetail{
                        OrderId = order.Id,
                        ItemId = ord.ItemId,
                        Quantity = ord.Quantity,
                    };
                    order.OrderDetails.Add(orderDetail);
                }
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                ex = new Exception(ex.Message);
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