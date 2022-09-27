using System;
using System.Collections.Generic;

#nullable disable

namespace ComercialClothes.Models
{
    public partial class Item
    {
        public Item()
        {
            Images = new HashSet<Image>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int CategoryId { get; set; }
        public int ShopId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime? DateCreated { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public int Quantity { get; set; }
        public virtual Category Category { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
