using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Models.DTOs;
using System.Linq;

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
           try
           {
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
        public async Task<bool> AddCart(List<CartRequest> req, int idAccount)
        {
            try
            {
                var findCart = await _orderRepository.GetCart(idAccount);
                // var findCart = await ConvertCart(req);
                if (findCart == null)
                {
                    foreach (var item in req)
                    {
                        await _unitOfWork.BeginTransaction();
                        var cart = new Order
                        {
                            AccountId = idAccount,
                            DateCreate = DateTime.UtcNow,
                            IsBought = false,
                            ShopId = item.ShopId,
                        };
                        await _orderRepository.AddAsync(cart);
                        foreach (var ord in item.OrderDetails)
                        {
                            var findItem = await _itemRepository.FindAsync(it => it.Id == ord.ItemId);
                            var orderDetail = new OrderDetail
                            {
                                OrderId = cart.Id,
                                ItemId = ord.ItemId,
                                Quantity = ord.Quantity,
                                Price = findItem.Price * ord.Quantity
                            };
                            cart.OrderDetails.Add(orderDetail);
                        }
                        await _unitOfWork.CommitTransaction();
                        // return true;   
                    }
                    return true;
                }
                await _unitOfWork.BeginTransaction();

                foreach (var removeOrder in findCart)
                {
                    await RemoveCart(removeOrder.Id);
                    _orderRepository.Delete(removeOrder);
                }
                foreach (var item in req)
                {
                    await _unitOfWork.BeginTransaction();
                    var cart = new Order
                    {
                        AccountId = idAccount,
                        DateCreate = DateTime.UtcNow,
                        IsBought = false,
                        ShopId = item.ShopId,
                    };
                    await _orderRepository.AddAsync(cart);
                    foreach (var ord in item.OrderDetails)
                    {
                        var findItem = await _itemRepository.FindAsync(it => it.Id == ord.ItemId);
                        var orderDetail = new OrderDetail
                        {
                            OrderId = cart.Id,
                            ItemId = ord.ItemId,
                            Quantity = ord.Quantity,
                            Price = findItem.Price * ord.Quantity
                        };
                        cart.OrderDetails.Add(orderDetail);
                    }
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
            var cart = await _orderRepository.GetCart(idAccount);
            if (cart == null)
            {
                return new List<CartResponse>();
            }
            var listCartResponse = new List<CartResponse>();

            foreach (var item in cart)
            {
                // Check listOrderDetail
                var listOrderDetail = item.OrderDetails.ToList();

                foreach (var ordetail in listOrderDetail)
                {
                    var imgItem = await _imageRepository.GetImage(ordetail.Item.Id);
                    // tao OrderDetailDTO
                    var oderDetailDTO = new OrderDetailDTO
                    {
                        Id = ordetail.Id,
                        Quantity = ordetail.Quantity.Value,
                        ItemName = ordetail.Item.Name,
                        Size = ordetail.Item.Size,
                        ItemId = ordetail.Item.Id,
                        ItemImg = imgItem.Path,
                        Price = ordetail.Item.Price * ordetail.Quantity.Value
                    };
                    // Kiem tra shop name da ton tai hay chua
                    var existedCartResponse = listCartResponse.Where(lcr => lcr.ShopName == ordetail.Item.Shop.Name)
                                            .FirstOrDefault();
                    if (existedCartResponse != null)
                    {
                        existedCartResponse.OrderDetails.Add(oderDetailDTO);
                    }

                    else
                    {
                        var cartResponse = new CartResponse();

                        // Luu Orderdetail vao cart response
                        cartResponse.ShopName = ordetail.Item.Shop.Name;
                        cartResponse.ShopId = ordetail.Item.ShopId;
                        var imgShop = await _imageRepository.GetImageByShopId(cartResponse.ShopId);
                        foreach (var img in imgShop)
                        {
                            cartResponse.ShopImage = img.Path;
                        }
                        // Tao moi
                        cartResponse.OrderDetails = new List<OrderDetailDTO>
                        {
                            oderDetailDTO
                        };

                        // Add vao cart response
                        listCartResponse.Add(cartResponse);
                    }
                }
            }
            return listCartResponse;
        }
    }
}