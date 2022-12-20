using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class UserRepository : Repository<Account>, IUserRepository
    {
        public UserRepository(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task<List<Account>> GetAccounts()
        {
            return await GetQuery(us => us.UserGroupId != 4 && us.IsActivated == true)
                        .ToListAsync();
        }

        public async Task<Account> GetNameAccount(int idShop)
        {
            return await GetQuery(sh => sh.ShopId == idShop).FirstOrDefaultAsync();
        }
    }
}
