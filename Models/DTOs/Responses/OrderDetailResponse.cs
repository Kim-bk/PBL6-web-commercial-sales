using CommercialClothes.Models.DTOs.Responses.Base;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class OrderDetailResponse : GeneralResponse
    {
        public List<OrderDetailDTO> OrderDetail;
    }
}
