using System;
using System.Collections.Generic;

#nullable disable

namespace ComercialClothes.Models
{
    public partial class Status
    {
        public Status()
        {
            Orders = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
