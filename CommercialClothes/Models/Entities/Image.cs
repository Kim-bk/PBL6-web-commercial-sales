using System;
using System.Collections.Generic;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Image
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public int? ShopId { get; set; }
        public string Path { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual Item Item { get; set; }
        public virtual Shop Shop { get; set; }
    }
}
