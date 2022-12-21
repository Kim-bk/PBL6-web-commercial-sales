using CommercialClothes.Models.Base;
using Model.Entities;
using System;
using System.Collections.Generic;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Status : BaseEntity
    {
        public Status()
        {
            Orders = new HashSet<Order>();
            HistoryTransactions = new HashSet<HistoryTransaction>();
        }
     
        public string Name { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<HistoryTransaction> HistoryTransactions { get; set; }
    }
}
