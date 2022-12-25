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
using Microsoft.AspNetCore.Mvc;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Responses;

namespace CommercialClothes.Services
{
    public class ItemService : BaseService, IItemService
    {
        private readonly IItemRepository _itemRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IUserRepository _userRepo;

        public ItemService(IItemRepository itemRepository ,IUnitOfWork unitOfWork, 
                           IImageRepository imageRepository,IMapperCustom mapper, IUserRepository userRepo) : base(unitOfWork, mapper)
        {
            _imageRepo = imageRepository;
            _itemRepo = itemRepository;
            _userRepo = userRepo;
        }

        public async Task<ItemResponse> AddItem(ItemRequest req, int accountId)
        {

            try
            {
                var findItem = await _itemRepo.FindAsync(it => it.Name == req.Name && it.CategoryId == req.CategoryId);
                if (findItem != null && findItem.CategoryId == req.CategoryId)
                {
                    return new ItemResponse()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Sản phẩm đã tồn tại!"
                    };
                }
                var account = await _userRepo.FindAsync(it => it.Id == accountId);
                await _unitOfWork.BeginTransaction();
                var item = new Item
                {
                    CategoryId = req.CategoryId,
                    ShopId = account.ShopId.Value,
                    Name = req.Name,
                    Price = req.Price,
                    DateCreated = DateTime.UtcNow,
                    Description = req.Description,
                    Size = req.Size, 
                    Quantity = req.Quantity
                };  
                await _itemRepo.AddAsync(item);
                foreach (var path in req.Paths)
                {
                    var img = new Image{
                        Path = path,
                        ItemId = item.Id
                    };
                    item.Images.Add(img);
                }
                await _unitOfWork.CommitTransaction();
                return new ItemResponse() 
                { 
                    IsSuccess = true,
                    ErrorMessage = "Thêm sản phẩm thành công!"
                };
            }
            catch (Exception ex)
            {
                return new ItemResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ItemResponse> AddItemAvailable(MoreItemRequest req, int accountId)
        {
            try
            {
                var account = await _userRepo.FindAsync(it => it.Id == accountId);
                foreach (var item in req.Items)
                {
                    var findItem = await _itemRepo.GetItemById(item);
                    await _unitOfWork.BeginTransaction();
                    var itemAdd = new Item
                    {
                        CategoryId = req.CategoryId,
                        ShopId = account.ShopId.Value,
                        Name = findItem.Name,
                        Price = findItem.Price,
                        DateCreated = DateTime.UtcNow,
                        Description = findItem.Description,
                        Size = findItem.Size, 
                        Quantity = findItem.Quantity
                    };  
                    await _itemRepo.AddAsync(itemAdd);
                    foreach(var path in findItem.Images)
                    {
                        var img = new Image{
                            Path = path.Path,
                            ItemId = itemAdd.Id    
                        };
                        itemAdd.Images.Add(img);
                    }
                    await _unitOfWork.CommitTransaction();
                    // return true;
                }
                return new ItemResponse()
                {
                    IsSuccess = true,
                };
            }
            catch (Exception ex)
            {
                return new ItemResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<List<ItemDTO>> GetAllItem()
        {
            var listItems = await _itemRepo.GetAll();
            return _mapper.MapItems(listItems);
        }
        public async Task<ItemDTO> GetItemById(int idItem)
        {
            var item = await _itemRepo.GetItemById(idItem);
            return new ItemDTO 
            {
                Id = item.Id,
                CategoryId = item.CategoryId, 
                ShopId = item.ShopId,
                ShopName = item.Shop.Name,
                Name = item.Name,
                Price = item.Price,
                DateCreated = item.DateCreated,
                Description = item.Description,
                Size = item.Size,
                Quantity = item.Quantity,
                CategoryName = item.Category.Name,
                Images = _mapper.MapImages((item.Images).ToList()),
            };
        }
        public async Task<ItemResponse> RemoveItemByItemId(int idItem)
        {
            try
            {
                var findImage = await _imageRepo.GetImageByItemId(idItem);
                var findItem = await _itemRepo.FindAsync(it => it.Id == idItem);
                if((findItem == null))
                {
                    return new ItemResponse()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tìm thấy sản phẩm!"
                    };
                }
                await _unitOfWork.BeginTransaction();
                foreach (var img in findImage)
                {
                   _imageRepo.Delete(img); 
                }
                _itemRepo.Delete(findItem);
                await _unitOfWork.CommitTransaction();
                return new ItemResponse()
                {
                    IsSuccess = true,
                };
                
            }
            catch (Exception ex)
            {
                return new ItemResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        public async Task<ItemResponse> UpdateItemByItemId(ItemRequest req, int accountId)
        {
            try
            {
                var itemReq = await _itemRepo.FindAsync(it => it.Id == req.Id);
                var images = await _imageRepo.GetImageByItemId(req.Id);
                var account = await _userRepo.FindAsync(it => it.Id == accountId);
                if(itemReq == null)
                {
                    return new ItemResponse()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tìm thấy sản phẩm!"
                    };
                }
                await _unitOfWork.BeginTransaction();
                foreach(var img in images)
                {
                    _imageRepo.Delete(img);
                }
                await _unitOfWork.CommitTransaction();
                await _unitOfWork.BeginTransaction();
                itemReq.CategoryId = req.CategoryId;
                itemReq.ShopId = account.ShopId.Value;
                itemReq.Name = req.Name;
                itemReq.Description = req.Description;
                itemReq.Size = req.Size;
                itemReq.Quantity = req.Quantity;
                foreach (var path in req.Paths)
                {
                    var pathImg = new Image{
                        ItemId = req.Id,
                        Path = path
                    };
                    itemReq.Images.Add(pathImg);
                }
                _itemRepo.Update(itemReq);
                await _unitOfWork.CommitTransaction();
                return new ItemResponse()
                {
                    IsSuccess = true
                };
            }
            catch (Exception ex)
            {
                return new ItemResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}