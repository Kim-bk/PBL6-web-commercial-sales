using CommercialClothes.Models.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DAL.Repositories
{
    public class refreshTokenRepo : Repository<RefreshToken>, IRefreshTokenRepository
    {
        public refreshTokenRepo(DbFactory dbFactory) : base(dbFactory)
        {
        }

        public async Task DeleteAll(int userId)
        {
            var listTokens = await GetQuery(u => u.UserId == userId).ToListAsync();
            Delete(listTokens);
        }
    }
}
