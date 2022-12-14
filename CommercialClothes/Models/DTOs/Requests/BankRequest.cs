using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class BankRequest
    {
        public int Id { get; set; }
        public string BankNumber { get; set; }
        public string AccountName { get; set; }
        public int BankTypeId {get; set;}
        public string? StartedDate { get; set; }
        public string? ExpiredDate { get; set; }   
    }
}