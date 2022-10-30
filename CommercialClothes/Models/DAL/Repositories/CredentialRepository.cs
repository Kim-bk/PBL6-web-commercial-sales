using CommercialClothes.Models.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class CredentialRepository : Repository<Credential>, ICredentialRepository
    {
        public CredentialRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }

        public async Task<List<string>> GetCredentialsByUserGroupId(int userGroupId)
        {
            return await GetQuery(cr => cr.UserGroupId == userGroupId)
                         .Select(cr => cr.Role.Name)
                         .ToListAsync();
        }
    }
}
