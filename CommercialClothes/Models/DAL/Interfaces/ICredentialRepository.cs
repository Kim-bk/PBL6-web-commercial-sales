using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DAL.Interfaces
{
    public interface ICredentialRepository : IRepository<Credential>
    {
        Task<List<string>> GetCredentialsByUserGroupId(int userId);
    }
}
