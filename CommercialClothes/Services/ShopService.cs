using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;
using Org.BouncyCastle.Ocsp;

namespace CommercialClothes.Services
{
    public class ShopService : BaseService, IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderDetailRepository _orderDetailRepository;
        public ShopService(IShopRepository shopRepository,IUnitOfWork unitOfWork, IMapperCustom mapper,
                           IImageRepository imageRepository,IUserRepository userRepository, IOrderRepository orderRepository
                           , IOrderDetailRepository orderDetailRepository) : base(unitOfWork, mapper)

        {
            _shopRepository = shopRepository;
            _imageRepository = imageRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<ShopResponse> AddShop(ShopRequest req, int idAccount)
        {
            try
            {
                var findShop = await _shopRepository.FindAsync(ca => ca.Name == req.Name);
                if (findShop != null)
                {
                    return new ShopResponse {
                        IsSuccess = false,
                        ErrorMessage = "Cửa hàng đã tồn tại!"
                    };
                }
                var findAccount = await _userRepository.FindAsync(us => us.Id == idAccount);
                if (findAccount.ShopId != null)
                {
                    return new ShopResponse{
                        IsSuccess = false,
                        ErrorMessage = "Không thể đăng ký nhiều cửa hàng!"
                    };
                }
                await _unitOfWork.BeginTransaction(); 
                var shop = new Shop
                {
                    Name = req.Name,
                    PhoneNumber = req.PhoneNumber,
                    DateCreated = DateTime.UtcNow, 
                    Address = req.Address,
                    Description = req.Description,
                };  
                foreach (var path in req.Paths)
                {
                    var img = new Image{
                        Path = path,
                        ShopId = shop.Id
                    };
                    shop.Images.Add(img);
                }
                var user = await _userRepository.FindAsync(us => us.Id == idAccount);
                user.Shop = shop;
                user.ShopId = shop.Id;
                user.UserGroupId = 3;
                _userRepository.Update(user);
                await _shopRepository.AddAsync(shop);
                await _unitOfWork.CommitTransaction();
                return new ShopResponse{
                    IsSuccess = true,
                };  
            }
            catch (Exception ex)
            {
                return new ShopResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<List<ShopDTO>> GetCategories(int idShop)
        {
            var shop = await _shopRepository.FindAsync(p => p.Id == idShop);
            var categoriesByShop = new List<ShopDTO>();
            var nameShop = await _userRepository.GetNameAccount(idShop);
            var items = new ShopDTO()
            {
                ShopId = shop.Id,
                Name = shop.Name,
                Address = shop.Address,
                PhoneNumber = shop.PhoneNumber,
                Description = shop.Description,
                NameAccount = nameShop.Name,
                DateCreated = shop.DateCreated,
                Categories = _mapper.MapCategories(shop.Categories.ToList()),
            };
            categoriesByShop.Add(items);
            return categoriesByShop;
        }
        
        public async Task<List<ShopDTO>> GetItemByShopId(int idShop)
        {
            var item = await _shopRepository.FindAsync(p => p.Id == idShop);
            var itemByShopId = new List<ShopDTO>();
            var nameShop = await _userRepository.GetNameAccount(idShop);
            var items = new ShopDTO()
            {
                ShopId = item.Id,
                Name = item.Name,
                Address = item.Address,
                PhoneNumber = item.PhoneNumber,
                NameAccount = nameShop.Name,
                Description = item.Description,
                DateCreated = item.DateCreated,
                Items = _mapper.MapItems(item.Items.DistinctBy(p => new{p.Name, p.Size}).ToList()),
            };
            itemByShopId.Add(items);
            return itemByShopId;
        }

        public async Task<List<OrderDTO>> GetOrder(int idUser)
        {
            var findShop = await _userRepository.FindAsync(us => us.Id == idUser);
            var findOrder = await _orderRepository.GetOrdersByShop(findShop.ShopId.Value);
            var lorder = new List<OrderDTO>();
            foreach(var item in findOrder)
            {
                var findOrderDetail = await _orderDetailRepository.ListOrderDetail(item.Id);
                var ordDetail = new List<OrderDetailDTO>();
                foreach(var ord in findOrderDetail)
                {
                    var imgItem = await _imageRepository.GetImage(ord.Item.Id);
                    var orderDetail = new OrderDetailDTO()
                    {
                        Id = ord.Id,
                        Quantity = ord.Quantity.Value,
                        ItemName = ord.Item.Name,
                        Size = ord.Item.Size,
                        ItemId = ord.Item.Id,
                        ItemImg = imgItem.Path,
                        Price = ord.Item.Price * ord.Quantity.Value
                    };
                    ordDetail.Add(orderDetail);
                }
                var order = new OrderDTO()
                {
                    Id = item.Id,
                    StatusId = item.StatusId.Value,
                    StatusName = item.Status.Name,
                    PaymentName = item.Payment.Type,
                    DateCreated = item.DateCreate,
                    PhoneNumber = item.PhoneNumber,
                    Address = item.Address,
                    OrderDetailsDTO = ordDetail,
                };
                lorder.Add(order);
            }
            return lorder;
        }

        public async Task<ShopDTO> GetShop(int idShop)
        {
            var findShop = await _shopRepository.FindAsync(sh => sh.Id == idShop);
            var imgShop = await _imageRepository.GetImageByShopId(idShop);
            var nameShop = await _userRepository.GetNameAccount(idShop);
            var shop = new ShopDTO()
            {
                ShopId = findShop.Id,
                Name = findShop.Name,
                Address = findShop.Address,
                PhoneNumber = findShop.PhoneNumber,
                NameAccount = nameShop.Name,
                Description = findShop.Description,
                DateCreated = findShop.DateCreated,
                Images = _mapper.MapImages(imgShop),
            };
            return shop;
        }


        public async Task<UserResponse> Login(LoginRequest req)
        {
            // 1. Find shop 
            var shop = await _userRepository.FindAsync(us => us.Email == req.Email && us.UserGroupId == 3);

            // 2. Check
            if (shop == null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Cần phải đăng nhập tài khoản Shop !",
                };
            }

            // 3. Check if user is activated
            if (!shop.IsActivated)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Vui lòng kiểm tra Email đã đăng ký để kích hoạt tài khoản !",
                };
            }

            // 4. Check if login password match
            if (_encryptor.MD5Hash(req.Password) != shop.Password)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Sai mật khẩu hoặc tên đăng nhập !",
                };
            }

            return new UserResponse
            {
                User = shop,
                IsSuccess = true
            };
        }

        public async Task<bool> UpdateShop(ShopRequest req, int accountId)
        {
            try
            {
                var findIdShop = await _userRepository.FindAsync(ish => ish.Id == accountId);
                var shopReq = await _shopRepository.FindAsync(it => it.Id == findIdShop.ShopId);
                var images = await _imageRepository.GetImageByShopId(findIdShop.ShopId.Value);
                if(shopReq == null)
                {
                    return new ShopResponse()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tim thấy shop"
                    };
                }
                await _unitOfWork.BeginTransaction();
                shopReq.Name = req.Name;
                shopReq.Address = req.Address;
                shopReq.PhoneNumber = req.PhoneNumber;
                shopReq.Description = req.Description;
                foreach (var path in req.Paths)
                {
                    foreach (var img in images)
                    {
                        if(path != img.Path)
                        {
                            var pathImg = new Image
                            {
                                Path = path
                            };
                            shopReq.Images.Add(pathImg);
                        }
                    }
                }
                _shopRepository.Update(shopReq);
                await _unitOfWork.CommitTransaction();
                return new ShopResponse()
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new ShopResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}