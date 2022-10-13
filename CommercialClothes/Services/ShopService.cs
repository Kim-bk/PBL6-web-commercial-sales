using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;
using CommercialClothes.Models.DTOs.Responses;
using CommercialClothes.Models.DTOs.Responsese;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services
{
    public class ShopService : BaseService, IShopService
    {
        private readonly IShopRepository _shopRepository;
        public ShopService(IShopRepository shopRepository,IUnitOfWork unitOfWork, IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _shopRepository = shopRepository;
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
        
    }
}