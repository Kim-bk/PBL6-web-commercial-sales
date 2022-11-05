using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Services
{
    public class CartService : BaseService, ICartService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public CartService(IUnitOfWork unitOfWork, IMapperCustom mapper, 
                           IOrderRepository orderRepository, IItemRepository itemRepository,
                           IOrderDetailRepository orderDetailRepository, IImageRepository imageRepository) : base(unitOfWork, mapper)
        {
            _orderRepository = orderRepository;
            _itemRepository = itemRepository;
            _orderDetailRepository = orderDetailRepository;
            _imageRepository = imageRepository;
        }
        public async Task<bool> RemoveCart(int idOrder){
           try{
            var findOrder = await _orderDetailRepository.ListOrderDetail(idOrder);
            foreach (var item in findOrder)
            {
                _orderDetailRepository.Delete(item);
            }
            return true;
           }
           catch(Exception ex)
           {
                ex = new Exception(ex.Message);
                throw ex;
           }
        }
        public async Task<bool> AddCart(CartRequest req,int idAccount){
            try
            {
                var findCartUser = await _orderRepository.FindAsync(us => us.AccountId == idAccount && us.PaymentId == null);
                if (findCartUser == null){
                    await _unitOfWork.BeginTransaction();
                    var cart = new Order
                    {
                        AccountId = idAccount,
                        DateCreate = DateTime.UtcNow,
                        IsBought = false,  
                    };
                    await _orderRepository.AddAsync(cart);
                    foreach (var ord in req.OrderDetails)
                    {
                        // var findItem = await _itemRepository.FindAsync(it => it.Name == ord.NameItem);
                        var orderDetail = new OrderDetail{
                            OrderId = cart.Id,
                            ItemId = ord.ItemId,
                            Quantity = ord.Quantity,
                        };
                        cart.OrderDetails.Add(orderDetail);
                    }
                    await _unitOfWork.CommitTransaction();
                    return true;
                }
                var findOrderDetail = await _orderDetailRepository.ListOrderDetail(findCartUser.Id);
                await _unitOfWork.BeginTransaction();
                findCartUser.DateCreate = DateTime.UtcNow;
                _orderRepository.Update(findCartUser);
                RemoveCart(findCartUser.Id);
                foreach (var ord in req.OrderDetails)
                {
                    // var findItem = await _itemRepository.FindAsync(it => it.Name == ord.NameItem);
                    var orderDetail = new OrderDetail{
                        OrderId = findCartUser.Id,
                        ItemId = ord.ItemId,
                        Quantity = ord.Quantity,
                    };
                    findCartUser.OrderDetails.Add(orderDetail);
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

        public async Task<List<CartResponse>> GetCartById(int idAccount)
        {
            //Check cart info
            var cart = await _orderRepository.FindAsync(
                            cr => cr.AccountId == idAccount && cr.IsBought == false);
            if(cart == null){
                return new List<CartResponse>();
            }
            // Check listOrderDetail
            var listOrderDetail = cart.OrderDetails.ToList();
          
            var listCartResponse = new List<CartResponse>();
                
            foreach (var item in listOrderDetail)
            {
                var imgItem = await _imageRepository.GetImage(item.Item.Id);
                // tao OrderDetailDTO
                var oderDetailDTO = new OrderDetailDTO
                {
                    OrderDetailId = item.Id,
                    QuantityOrderDetail = item.Quantity.Value,
                    ItemName = item.Item.Name,
                    Size = item.Item.Size,
                    ItemId = item.Item.Id,
                    ItemImg = imgItem.Path,
                    Price = item.Item.Price * item.Quantity.Value
                };
                // Kiem tra shop name da ton tai hay chua
                var existedCartResponse = listCartResponse.Where(lcr => lcr.ShopName == item.Item.Shop.Name)
                                           .FirstOrDefault();
                if (existedCartResponse != null)
                {
                    existedCartResponse.OrderDetails.Add(oderDetailDTO);
                }

                else
                {
                    var cartResponse = new CartResponse();

                    // Luu Orderdetail vao cart response
                    cartResponse.ShopName = item.Item.Shop.Name;
                    cartResponse.ShopId = item.Item.ShopId;
                    var imgShop = await _imageRepository.GetImageByShopId(cartResponse.ShopId);
                    foreach (var img in imgShop)
                    {
                        cartResponse.ShopImage = img.Path;
                    }
                    // Tao moi
                    cartResponse.OrderDetails = new List<OrderDetailDTO>();
                    cartResponse.OrderDetails.Add(oderDetailDTO);

                    // Add vao cart response
                    listCartResponse.Add(cartResponse);
                }         
            } 
            return listCartResponse;  
        }
    }
}