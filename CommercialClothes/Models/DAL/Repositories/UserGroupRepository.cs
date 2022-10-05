using ComercialClothes.Models;
using ComercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Interfaces;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class UserGroupRepository : Repository<UserGroup>, IUserGroupRepository
    {
        public UserGroupRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
