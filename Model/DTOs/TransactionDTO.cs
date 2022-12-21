using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DTOs
{
    public class TransactionDTO
    {
        public int CustomerId { get; set; }
        public int ShopId { get; set; }
        public int Money { get; set; }
    }
}