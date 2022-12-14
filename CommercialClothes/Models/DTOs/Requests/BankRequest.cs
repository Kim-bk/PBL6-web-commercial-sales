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
        public string BankName { get; set; }
        public string UserName { get; set; }
        public int AccountId { get; set; }
        public string? StartedDate { get; set; }
        public string? ExpiredDate { get; set; }   
    }
}