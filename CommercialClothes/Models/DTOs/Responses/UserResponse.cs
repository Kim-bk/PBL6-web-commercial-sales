namespace CommercialClothes.Models.DTOs.Responses
{
    public class UserResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMesage { get; set; }
        public Account User { get; set; }
    }
}
