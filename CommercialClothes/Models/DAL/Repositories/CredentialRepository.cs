using CommercialClothes.Models.DAL.Interfaces;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class CredentialRepository : Repository<Credential>, ICredentialRepository
    {
        public CredentialRepository(DbFactory dbFactory) : base(dbFactory)
        {

        }
    }
}
