using System.Threading.Tasks;

namespace CommercialClothes.Models.DAL.Interfaces
{
    public interface IRefreshTokenRepository : IRepository<RefreshToken>
    {
        public Task DeleteAll(int userId);
    }
}