using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DAL.Repositories
{
    public interface IUserRepository : IRepository<Account>
    {
        public Task<Account> GetNameAccount(int idShop);
        public Task<List<Account>> GetAccounts();
    }
}
