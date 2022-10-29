using System.Collections.Generic;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class CartResponse
    {
        public string ShopName {get; set;}
        public List<OrderDetailDTO> OrderDetails {get;set;}
        
    }
}