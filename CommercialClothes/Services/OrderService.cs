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
            , IMapperCustom mapper,IOrderDetailRepository orderDetailRepository) : base(unitOfWork, mapper)
        {
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<bool> AddOrder(OrderRequest req,int idAccount)
        {
            try
            {
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
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> CancelOrder(int orderId)
        {
            try
            {
                var findOrder = await _orderRepository.FindAsync(or => or.Id == orderId);
                await _unitOfWork.BeginTransaction();
                findOrder.StatusId = 4;
                _orderRepository.Update(findOrder);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (System.Exception)
            {                
                throw;
            }
        }

        public async Task<bool> UpdateStatusOrder(int orderId)
        {
            try
            {
                var findOrder = await _orderRepository.FindAsync(or => or.Id == orderId);
                if (findOrder.StatusId < 3)
                {
                    await _unitOfWork.BeginTransaction();
                    findOrder.StatusId = findOrder.StatusId + 1;
                    _orderRepository.Update(findOrder);
                    await _unitOfWork.CommitTransaction();
                    return true;
                }
                return false;
            }
            catch (System.Exception)
            {                
                throw;
            }
        }
        
    }
}