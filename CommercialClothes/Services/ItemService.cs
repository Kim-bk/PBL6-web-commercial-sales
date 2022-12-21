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
        private readonly IItemRepository _itemRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;

        public ItemService(IItemRepository itemRepository ,IUnitOfWork unitOfWork, 
                           IImageRepository imageRepository,IMapperCustom mapper, IUserRepository userRepository) : base(unitOfWork, mapper)
        {
            _imageRepository = imageRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
        }

        public async Task<ItemResponse> AddItem(ItemRequest req, int accountId)
        {

            try
            {
                var findItem = await _itemRepository.FindAsync(it => it.Name == req.Name && it.CategoryId == req.CategoryId);
                if (findItem != null && findItem.CategoryId == req.CategoryId)
                {
                    return new ItemResponse()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Sản phẩm đã tồn tại!"
                    };
                }
                var account = await _userRepository.FindAsync(it => it.Id == accountId);
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
                await _itemRepository.AddAsync(item);
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
                var account = await _userRepository.FindAsync(it => it.Id == accountId);
                foreach (var item in req.Items)
                {
                    var findItem = await _itemRepository.GetItemById(item);
                    await _unitOfWork.BeginTransaction();
                    foreach(var itemA in findItem)
                    {
                        var itemAdd = new Item
                        {
                            CategoryId = req.CategoryId,
                            ShopId = account.ShopId.Value,
                            Name = itemA.Name,
                            Price = itemA.Price,
                            DateCreated = DateTime.UtcNow,
                            Description = itemA.Description,
                            Size = itemA.Size, 
                            Quantity = itemA.Quantity
                        };  
                        await _itemRepository.AddAsync(itemAdd);
                        foreach(var path in itemA.Images)
                        {
                            var img = new Image{
                                Path = path.Path,
                                ItemId = itemAdd.Id    
                            };
                            itemAdd.Images.Add(img);
                        }
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
            var listItems = await _itemRepository.GetAll();
            return _mapper.MapItems(listItems);
        }
        public async Task<List<ItemDTO>> GetItemById(int idItem)
        {
            var item = await _itemRepository.GetItemById(idItem);
            return _mapper.MapItems(item);
        }
        public async Task<ItemResponse> RemoveItemByItemId(int idItem)
        {
            try
            {
                var findImage = await _imageRepository.GetImageByItemId(idItem);
                var findItem = await _itemRepository.FindAsync(it => it.Id == idItem);
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
                   _imageRepository.Delete(img); 
                }
                _itemRepository.Delete(findItem);
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
                var itemReq = await _itemRepository.FindAsync(it => it.Id == req.Id);
                var images = await _imageRepository.GetImageByItemId(req.Id);
                var account = await _userRepository.FindAsync(it => it.Id == accountId);
                if(itemReq == null)
                {
                    return new ItemResponse()
                    {
                        IsSuccess = false,
                        ErrorMessage = "Không tìm thấy sản phẩm!"
                    };
                }
                await _unitOfWork.BeginTransaction();
                itemReq.CategoryId = req.CategoryId;
                itemReq.ShopId = account.ShopId.Value;
                itemReq.Name = req.Name;
                itemReq.Description = req.Description;
                itemReq.Size = req.Size;
                itemReq.Quantity = req.Quantity;
                foreach (var path in req.Paths)
                {
                    foreach (var img in images)
                    {
                        if(path != img.Path)
                        {
                            var pathImg = new Image{
                                Path = path
                            };
                            itemReq.Images.Add(pathImg);
                        }
                    }
                }
                _itemRepository.Update(itemReq);
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