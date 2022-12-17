using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Responses.Base;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class BankResponse : GeneralResponse
    {
        public List<BankDTO> UserBanks { get; set; }
    }
}