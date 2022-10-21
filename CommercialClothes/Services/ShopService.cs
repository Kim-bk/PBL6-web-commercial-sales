using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Models.DTOs.Responsese;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services
{
    public class ShopService : BaseService, IShopService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;
        public ShopService(IShopRepository shopRepository,IUnitOfWork unitOfWork, IMapperCustom mapper,
                           IImageRepository imageRepository) : base(unitOfWork, mapper)
        {
            _shopRepository = shopRepository;
            _imageRepository = imageRepository;
        }

        public async Task<bool> AddShop(ShopRequest req)
        {
            try
            {
                var findShop = await _shopRepository.FindAsync(ca => ca.Name == req.Name);
                if (findShop != null)
                {
                    throw new Exception("Category is already existed!");
                }
                await _unitOfWork.BeginTransaction(); 
                var shop = new Shop
                {
                    Name = req.Name,
                    PhomeNumber = req.PhoneNumber,
                    DateCreated = DateTime.UtcNow, 
                };  
                foreach (var path in req.Paths)
                {
                    var img = new Image{
                        Path = path,
                        ShopId = shop.Id
                    };
                    shop.Images.Add(img);
                }
                var user = await _userRepository.FindAsync(us => us.Id == req.IdUser);
                user.Shop = shop;
                await _shopRepository.AddAsync(shop);
                await _unitOfWork.CommitTransaction();
                return true;  
            }
            catch (System.Exception e)
            {
                throw e;
            }
        }

        public async Task<List<ShopDTO>> GetCategories(int idShop)
        {
            var shop = await _shopRepository.FindAsync(p => p.Id == idShop);
            var categoriesByShop = new List<ShopDTO>();
            if (shop == null)
            {
                throw new Exception("Shop not found!!!!!!!");
            } 
            var items = new ShopDTO()
            {
                Id = shop.Id,
                Name = shop.Name,
                Categories = GetCategoriesByShop(shop.Categories.ToList()),
            };
            categoriesByShop.Add(items);
            return categoriesByShop;
        }

        public List<CategoryDTO> GetCategoriesByShop(List<Category> categories)
        {
            return _mapper.MapCategories(categories);
        }

        public List<ItemDTO> GetItemByShop(List<Item> items)
        {
            return _mapper.MapItems(items);
        }
        
        public async Task<List<ShopDTO>> GetItemByShopId(int idShop)
        {
            var item = await _shopRepository.FindAsync(p => p.Id == idShop);
            var itemByShopId = new List<ShopDTO>();
            if (item == null)
            {
                throw new Exception("Item not found!!!!!!!");
            }
            var items = new ShopDTO()
            {
                Id = item.Id,
                Name = item.Name,
                Items = GetItemByShop(item.Items.ToList()),
            };
            itemByShopId.Add(items);
            return itemByShopId;
        }

        public async Task<bool> UpdateShop(ShopRequest req)
        {
            try
            {
                var shopReq = await _shopRepository.FindAsync(it => it.Id == req.Id);
                var images = await _imageRepository.GetImageByShopId(req.Id);
                if(shopReq == null)
                {
                    throw new Exception("Shop not found!!");
                }
                await _unitOfWork.BeginTransaction();
                shopReq.Name = req.Name;
                shopReq.Address = req.Address;
                shopReq.PhomeNumber = req.PhoneNumber;
                foreach (var path in req.Paths)
                {
                    foreach (var img in images)
                    {
                        if(path != img.Path)
                        {
                            var pathImg = new Image{
                                Path = path
                            };
                            shopReq.Images.Add(pathImg);
                        }
                    }
                }
                _shopRepository.Update(shopReq);
                await _unitOfWork.CommitTransaction();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}