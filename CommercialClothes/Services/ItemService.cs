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

namespace CommercialClothes.Services
{
    public class ItemService : BaseService, IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IImageRepository _imageRepository;
        public ItemService(IItemRepository itemRepository ,IUnitOfWork unitOfWork, 
                           IImageRepository imageRepository,IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _imageRepository = imageRepository;
            _itemRepository = itemRepository;
        }
        public async Task<bool> AddItem(ItemRequest req)
        {
            try
            {
                var findItem = await _itemRepository.FindAsync(it => it.Name == req.Name);
               
                if (findItem != null && findItem.CategoryId == req.CategoryId && findItem.ShopId == req.ShopId)
                {
                    throw new Exception("Item is already existed!");
                }

                await _unitOfWork.BeginTransaction();
                var item = new Item
                {
                    CategoryId = req.CategoryId,
                    ShopId = req.ShopId,
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
                return true;
            }
            catch (Exception e)
            {
                throw e;
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
            if (item == null)
            {
                throw new Exception("Item not found!!!!!!!");
            }
            return _mapper.MapItems(item);
            // return itembyId;       
        }
        public async Task<bool> RemoveItemByItemId(int idItem)
        {
            try
            {
                var findImage = await _imageRepository.GetImageByItemId(idItem);
                var findItem = await _itemRepository.FindAsync(it => it.Id == idItem);
                if((findItem == null))
                {
                    throw new Exception("Item not found!!");
                }
                await _unitOfWork.BeginTransaction();
                foreach (var img in findImage)
                {
                   _imageRepository.Delete(img); 
                }
                _itemRepository.Delete(findItem);
                await _unitOfWork.CommitTransaction();
                return true;
                
            }
            catch (Exception e)
            {
                throw e; 
            }
        }

        public async Task<bool> UpdateItemByItemId(ItemRequest req)
        {
            try
            {
                var itemReq = await _itemRepository.FindAsync(it => it.Id == req.Id);
                var images = await _imageRepository.GetImageByItemId(req.Id);
                if(itemReq == null)
                {
                    throw new Exception("Item not found!!");
                }
                await _unitOfWork.BeginTransaction();
                itemReq.CategoryId = req.CategoryId;
                itemReq.ShopId = req.ShopId;
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
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}