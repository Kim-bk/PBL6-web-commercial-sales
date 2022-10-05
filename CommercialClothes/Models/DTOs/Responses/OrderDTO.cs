using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int StatusId { get; set; }
        public int PaymentId { get; set; }
        public DateTime DateCreate { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string Account { get; set; }
        
    }
}