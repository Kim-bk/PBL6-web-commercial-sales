using CommercialClothes.Models.Base;
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
        }

     
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
