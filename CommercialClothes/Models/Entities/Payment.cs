using System;
using System.Collections.Generic;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Ordereds = new HashSet<Order>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<Order> Ordereds { get; set; }
    }
}
