using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services
{
    public class StatisticalService : BaseService, IStatisticalService
    {
        private readonly IShopRepository _shopRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUserRepository _userRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IOrderRepository _orderRepository;
        public StatisticalService(IUnitOfWork unitOfWork, IMapperCustom mapper,IShopRepository shopRepository
                                  ,IImageRepository imageRepository, IUserRepository userRepository, IItemRepository itemRepository
                                  ,IOrderRepository orderRepository) : base(unitOfWork, mapper)
        {
            _shopRepository = shopRepository;
            _imageRepository = imageRepository;
            _itemRepository = itemRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
        }

        public async Task<List<StatisticalDTO>> ListItemSoldByInterval(StatisticalRequest req)
        {
            var listStatistical = new List<StatisticalDTO>();
            if(Int32.Parse(req.From.Substring(0,4)) < Int32.Parse(req.To.Substring(0,4)))
            {
                for(int i = Int32.Parse(req.From.Substring(0,4)); i <= Int32.Parse(req.To.Substring(0,4)); i++)
                {
                    var listItemInMonth = await _orderRepository.GetOrdersByDate(i.ToString(),req.ShopId);
                    if(listItemInMonth == null){
                        return new List<StatisticalDTO>();
                    }
                    foreach (var litem in listItemInMonth)
                    {
                        foreach (var item in litem.OrderDetails)
                        {
                            if(item.Item.ShopId == req.ShopId)
                            {
                                var itemMonth = new StatisticalDTO
                                {
                                    ItemId = item.ItemId,
                                    NameItem = item.Item.Name,
                                    From = req.From,
                                    To = req.To,
                                    CountSold = item.Quantity.Value,
                                };
                                var existedItem = listStatistical.Where(exi => exi.ItemId == item.ItemId).FirstOrDefault();
                                if(existedItem != null)
                                {
                                    existedItem.CountSold += item.Quantity.Value; 
                                    existedItem.Turnover += item.Price;
                                }
                                else
                                {
                                    itemMonth.Turnover = item.Price;
                                    listStatistical.Add(itemMonth);
                                }
                            }
                        }
                    }
                }
            }
            return listStatistical;
        }

        public async Task<List<StatisticalDTO>> ListItemsSold(int idShop)
        {
            var request = await _itemRepository.GetItems(idShop);
            var listStatistical = new List<StatisticalDTO>();
            foreach (var item in request)
            {
                var date = new StatisticalDTO{
                    NameItem = item.Name,
                    From = item.DateCreated.ToString(),
                    To = DateTime.UtcNow.ToString(),
                    CountSold = item.Quantity,
                };
                listStatistical.Add(date);
            };
            return listStatistical;
        }

        public async Task<List<StatisticalDTO>> ListItemsSoldByDate(int idShop, string date)
        {
            var listItemInMonth = await _orderRepository.GetOrdersByDate(date, idShop);
            if(listItemInMonth == null){
                return new List<StatisticalDTO>();
            }
            var listStatistical = new List<StatisticalDTO>();
            foreach (var litem in listItemInMonth)
            {
                foreach (var item in litem.OrderDetails)
                {
                    if(item.Item.ShopId == idShop)
                    {
                        var itemMonth = new StatisticalDTO
                        {
                            ItemId = item.ItemId,
                            NameItem = item.Item.Name,
                            CountSold = item.Quantity.Value,
                        };
                        var existedItem = listStatistical.Where(exi => exi.ItemId == item.ItemId).FirstOrDefault();
                        if(existedItem != null)
                        {
                            existedItem.CountSold += item.Quantity.Value;
                        }
                        else
                        {
                            listStatistical.Add(itemMonth);
                        }
                    }
                }
            }
            return listStatistical;
        }
    }
}