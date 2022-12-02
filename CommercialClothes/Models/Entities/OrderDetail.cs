using CommercialClothes.Models.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class OrderDetail : BaseEntity
    {
        public int OrderId { get; set; }
        public int ItemId { get; set; }
        public int? Quantity { get; set; }
        public int Price { get; set; }
        public virtual Item Item { get; set; }
        public virtual Order Order { get; set; }
    }
}
