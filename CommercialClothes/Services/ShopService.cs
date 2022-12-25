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
using Model.DAL.Interfaces;
using Model.DTOs.Responses;
using Org.BouncyCastle.Ocsp;

namespace CommercialClothes.Services
{
    public class ShopService : BaseService, IShopService
    {
        private readonly IShopRepository _shopRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IUserRepository _userRepo;
        private readonly IOrderRepository _orderRepo;
        private readonly IOrderDetailRepository _orderDetailRepo;
        private readonly IHistoryTransactionRepository _historyTransactionRepo;
        private readonly Encryptor _encryptor;

        public ShopService(IShopRepository shopRepository, IUnitOfWork unitOfWork
            , IImageRepository imageRepository, IUserRepository userRepository, IMapperCustom mapper
            , IOrderRepository orderRepository, IOrderDetailRepository orderDetailRepository
            , Encryptor encryptor, IHistoryTransactionRepository historyTransactionRepo) : base(unitOfWork, mapper)

        {
            _shopRepo = shopRepository;
            _imageRepo = imageRepository;
            _userRepo = userRepository;
            _orderRepo = orderRepository;
            _orderDetailRepo = orderDetailRepository;
            _historyTransactionRepo = historyTransactionRepo;
            _encryptor = encryptor;
        }

        public async Task<ShopResponse> AddShop(ShopRequest req, int idAccount)
        {
            try
            {
                var findShop = await _shopRepo.FindAsync(ca => ca.Name == req.Name);
                if (findShop != null)
                {
                    return new ShopResponse
                    {
                        IsSuccess = false,
                        ErrorMessage = "Cửa hàng đã tồn tại!"
                    };
                }
                var findAccount = await _userRepo.FindAsync(us => us.Id == idAccount);
                if (findAccount.ShopId != null)
                {
                    return new ShopResponse
                    {
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
                    var img = new Image
                    {
                        Path = path,
                        ShopId = shop.Id
                    };
                    shop.Images.Add(img);
                }
                var user = await _userRepo.FindAsync(us => us.Id == idAccount);
                user.Shop = shop;
                user.ShopId = shop.Id;
                user.UserGroupId = 3;
                _userRepo.Update(user);
                await _shopRepo.AddAsync(shop);
                await _unitOfWork.CommitTransaction();
                return new ShopResponse
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

        public async Task<List<ShopDTO>> GetCategories(int idShop)
        {
            var shop = await _shopRepo.FindAsync(p => p.Id == idShop);
            var categoriesByShop = new List<ShopDTO>();
            var nameShop = await _userRepo.GetNameAccount(idShop);
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
            var item = await _shopRepo.FindAsync(p => p.Id == idShop);
            var itemByShopId = new List<ShopDTO>();
            var nameShop = await _userRepo.GetNameAccount(idShop);
            var items = new ShopDTO()
            {
                ShopId = item.Id,
                Name = item.Name,
                Address = item.Address,
                PhoneNumber = item.PhoneNumber,
                NameAccount = nameShop.Name,
                Description = item.Description,
                DateCreated = item.DateCreated,
                Items = _mapper.MapItems(item.Items.DistinctBy(p => new { p.Name, p.Size }).ToList()),
            };
            itemByShopId.Add(items);
            return itemByShopId;
        }

        public async Task<List<OrderDTO>> GetOrder(int idUser)
        {
            var findShop = await _userRepo.FindAsync(us => us.Id == idUser);
            var findOrder = await _orderRepo.GetOrdersByShop(findShop.ShopId.Value);
            var lorder = new List<OrderDTO>();
            foreach (var item in findOrder)
            {
                var findOrderDetail = await _orderDetailRepo.ListOrderDetail(item.Id);
                var ordDetail = new List<OrderDetailDTO>();
                foreach (var ord in findOrderDetail)
                {
                    var imgItem = await _imageRepo.GetImage(ord.Item.Id);
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
                    OrderDetails = ordDetail,
                };
                lorder.Add(order);
            }
            return lorder;
        }

        public async Task<ShopDTO> GetShop(int idShop)
        {
            var findShop = await _shopRepo.FindAsync(sh => sh.Id == idShop);
            var imgShop = await _imageRepo.GetImageByShopId(idShop);
            var nameShop = await _userRepo.GetNameAccount(idShop);
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

        public async Task<int> GetShopWallet(int shopId)
        {
            return (await _shopRepo.FindAsync(s => s.Id == shopId)).ShopWallet.HasValue == false ? 0
                : (await _shopRepo.FindAsync(s => s.Id == shopId)).ShopWallet.Value;
            
        }

        public async Task<List<TransactionResponse>> GetTransactions(int shopId)
        {
            var result = new List<TransactionResponse>();
            var allTransactions = await _historyTransactionRepo.GetTransactionsOfShop(shopId);
            var shopName = (await _shopRepo.FindAsync(s => s.Id == shopId)).Name;
            foreach (var transaction in allTransactions)
            {
                var transactionRes = new TransactionResponse
                {
                    BillId = transaction.BillId,
                    ShopName = shopName,
                    CustomerName = (await _userRepo.FindAsync(us => us.Id == transaction.CustomerId)).Name,
                    TransactionDate = transaction.TransactionDate,
                    Money = "+" + transaction.Money.ToString(),
                    Status = "Đã Giao"
                };

                result.Add(transactionRes);
            }
            return result.OrderByDescending(rs => rs.TransactionDate).ToList();
        }

        public async Task<UserResponse> Login(LoginRequest req)
        {
            // 1. Find shop
            var shop = await _userRepo.FindAsync(us => us.Email == req.Email && (us.UserGroupId == 3 || us.UserGroupId == 4));

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

        public async Task<ShopResponse> UpdateShop(ShopRequest req, int accountId)
        {
            try
            {
                var findIdShop = await _userRepo.FindAsync(ish => ish.Id == accountId);
                var shopReq = await _shopRepo.FindAsync(it => it.Id == findIdShop.ShopId);
                var images = await _imageRepo.GetImageByShopId(findIdShop.ShopId.Value);
                if (shopReq == null)
                {
                    return new ShopResponse()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tim thấy shop"
                    };
                }
                await _unitOfWork.BeginTransaction();
                foreach(var img in images)
                {
                    _imageRepo.Delete(img);
                }
                await _unitOfWork.CommitTransaction();
                await _unitOfWork.BeginTransaction();
                shopReq.Name = req.Name;
                shopReq.Address = req.Address;
                shopReq.PhoneNumber = req.PhoneNumber;
                shopReq.Description = req.Description;
                foreach (var path in req.Paths)
                {
                        var pathImg = new Image
                        {
                            ShopId = findIdShop.ShopId,
                            Path = path
                        };
                        shopReq.Images.Add(pathImg);
                }
                _shopRepo.Update(shopReq);
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