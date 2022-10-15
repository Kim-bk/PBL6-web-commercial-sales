namespace CommercialClothes.Models.DTOs.Responses
{
    public class RefreshTokenResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public RefreshToken RefreshToken { get; set; }
    }
}
