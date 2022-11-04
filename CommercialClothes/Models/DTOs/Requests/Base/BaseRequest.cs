using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests.Base
{
    public class BaseRequest
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}