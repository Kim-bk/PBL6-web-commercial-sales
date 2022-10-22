using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class ImageRepository : Repository<Image>, IImageRepository
    {
        public ImageRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Image>> GetImageByItemId(int idItem)
        {
            return await GetQuery(it => it.ItemId == idItem).ToListAsync();

           // return await DbSet.Where(it => it.ItemId == idItem).ToListAsync();
        }

        public async Task<List<Image>> GetImageByShopId(int idShop)
        {
            return await GetQuery(sh => sh.ShopId == idShop).ToListAsync();
        }
    }
}