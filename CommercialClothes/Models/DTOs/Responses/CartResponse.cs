using System.Collections.Generic;
using CommercialClothes.Models.DTOs.Responses.Base;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class CartResponse : GeneralResponse
    {
        public string ShopName {get; set;}
        public int ShopId { get; set; }
        public string ShopImage { get; set; }
        public List<OrderDetailDTO> OrderDetails {get;set;}
        
    }
}