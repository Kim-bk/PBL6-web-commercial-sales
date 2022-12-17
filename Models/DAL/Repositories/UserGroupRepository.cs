using ComercialClothes.Models;
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
