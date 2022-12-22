using System.ComponentModel.DataAnnotations;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class RefreshTokenRequest
    {
        public string Token { get; set; }
    }
}
