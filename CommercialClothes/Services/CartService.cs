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
        private readonly IOrderRepository _orderRepo;
        private readonly IItemRepository _itemRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;

        public CartService(IUnitOfWork unitOfWork, IMapperCustom mapper,
                           IOrderRepository orderRepository, IItemRepository itemRepository,
                           IOrderDetailRepository orderDetailRepository, IImageRepository imageRepository) : base(unitOfWork, mapper)
        {
            _orderRepo = orderRepository;
            _itemRepo = itemRepository;
            _orderDetailRepo = orderDetailRepository;
            _imageRepo = imageRepository;
        }

        public async Task<bool> RemoveCart(int idOrder)
        {
            try
            {
                var findOrder = await _orderDetailRepo.ListOrderDetail(idOrder);
                foreach (var item in findOrder)
                {
                    _orderDetailRepo.Delete(item);
                }
                return true;
            }
            catch (Exception ex)
            {
                ex = new Exception(ex.Message);
                throw ex;
            }
        }

        public async Task<CartResponse> AddCart(List<CartRequest> req, int idAccount)
        {
            try
            {
                var findCart = await _orderRepo.GetCart(idAccount);
                // var findCart = await ConvertCart(req);
                if (findCart.Count == 0)
                {
                    foreach(var item in req)
                    {
                        await _unitOfWork.BeginTransaction();
                        var cart = new Order
                        {
                            AccountId = idAccount,
                            DateCreate = DateTime.UtcNow,
                            IsBought = false,
                            ShopId = item.ShopId,
                        };
                        await _orderRepo.AddAsync(cart);
                        await _unitOfWork.CommitTransaction();
                        foreach(var ord in item.OrderDetails)
                        {
                            var findItem = await _itemRepo.FindAsync(it => it.Id == ord.ItemId);
                            var orderDetail = new OrderDetail
                            {
                                OrderId = cart.Id,
                                ItemId = ord.ItemId.Value,
                                Quantity = ord.Quantity.Value,
                                Price = findItem.Price * ord.Quantity.Value
                            };
                            cart.OrderDetails.Add(orderDetail);
                        }
                        await _unitOfWork.CommitTransaction();
                        // return true;
                    }
                    return new CartResponse{
                        IsSuccess = true,
                    };
                }
                await _unitOfWork.BeginTransaction();

                foreach (var removeOrder in findCart)
                {
                    await RemoveCart(removeOrder.Id);
                    _orderRepo.Delete(removeOrder);
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
                    await _orderRepo.AddAsync(cart);
                    foreach (var ord in item.OrderDetails)
                    {
                        var findItem = await _itemRepo.FindAsync(it => it.Id == ord.ItemId);
                        var orderDetail = new OrderDetail
                        {
                            OrderId = cart.Id,
                            ItemId = ord.ItemId.Value,
                            Quantity = ord.Quantity.Value,
                            Price = findItem.Price * ord.Quantity.Value
                        };
                        cart.OrderDetails.Add(orderDetail);
                    }
                }
                await _unitOfWork.CommitTransaction();
                return new CartResponse{
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new CartResponse
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message,
                };
            }
        }

        public async Task<List<CartResponse>> GetCartById(int idAccount)
        {
            //Check cart info
            var cart = await _orderRepo.GetCart(idAccount);
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
                    var imgItem = await _imageRepo.GetImage(ordetail.Item.Id);
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
                        var imgShop = await _imageRepo.GetImageByShopId(cartResponse.ShopId);
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