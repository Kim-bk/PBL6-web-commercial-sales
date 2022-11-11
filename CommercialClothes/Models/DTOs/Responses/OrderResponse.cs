using CommercialClothes.Models.DTOs.Responses.Base;
using System.Collections.Generic;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class OrderResponse : GeneralResponse
    {
        public List<OrderDTO> OrdersDTO {get ;set; }
    }
}