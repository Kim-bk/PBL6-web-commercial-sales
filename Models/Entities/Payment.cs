using System;
using System.Collections.Generic;

#nullable disable

namespace ComercialClothes.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Ordereds = new HashSet<Ordered>();
        }

        public int Id { get; set; }
        public string Type { get; set; }

        public virtual Shop IdNavigation { get; set; }
        public virtual ICollection<Ordered> Ordereds { get; set; }
    }
}
