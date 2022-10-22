using CommercialClothes.Models.DTOs.Responses.Base;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class UserResponse : GeneralResponse
    {
        public Account User { get; set; }
    }
}
