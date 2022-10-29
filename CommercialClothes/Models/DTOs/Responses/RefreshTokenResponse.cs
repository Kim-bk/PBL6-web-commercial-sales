using CommercialClothes.Models.DTOs.Responses.Base;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class RefreshTokenResponse : GeneralResponse
    {
        public RefreshToken RefreshToken { get; set; }
    }
}
