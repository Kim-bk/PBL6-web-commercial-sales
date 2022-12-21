using System.ComponentModel.DataAnnotations;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class OrderRequest
    {
        [Required]
        public int PaymentId { get; set; }

        public int Total { get; set; }

        [Required]
        public int ShopId { get; set; }

        [Required]
        public string BankCode { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Address { get; set; }

        public string City { get; set; }
        public string Country { get; set; }

        [Required]
        public List<CartRequest> Details { get; set; }
    }
}