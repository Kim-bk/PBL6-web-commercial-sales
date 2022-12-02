using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommercialClothes.Models.DTOs.Responses.Base;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class IntervalResponse : GeneralResponse
    {
        public string Title { get; set; }
        public List<string> Labels { get; set; }
        public List<int> Data { get; set; }
    }
}