using System;
using System.Collections.Generic;

#nullable disable

namespace ComercialClothes.Models
{
    public partial class Status
    {
        public Status()
        {
            Ordereds = new HashSet<Ordered>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Ordered> Ordereds { get; set; }
    }
}
