using CommercialClothes.Models;
using CommercialClothes.Models.DTOs.Responses;
using System.Threading.Tasks;

namespace CommercialClothes.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<TokenResponse> Authenticate(Account user);
    }
}
