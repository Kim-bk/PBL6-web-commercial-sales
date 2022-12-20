using ComercialClothes.Models;
using CommercialClothes.Models.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class UserGroupRepository : Repository<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<UserGroup>> GetMainUserGroup()
        {
            // admin - customer - shop
            return await GetQuery(ug => ug.Id == 1 || ug.Id == 3 || ug.Id == 2).ToListAsync();
        }
    }
}
