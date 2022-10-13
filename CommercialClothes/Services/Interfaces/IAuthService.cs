using CommercialClothes.Models;
using CommercialClothes.Models.DTOs;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenResponse> Authenticate(Account usser);
    }
}
