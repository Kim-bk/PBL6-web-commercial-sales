using CommercialClothes.Models.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Category : BaseEntity
    {
        public Category()
        {
            Items = new HashSet<Item>();
        }

        public int? ParentId { get; set; }
        public int? ShopId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Gender { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual ICollection<Item> Items { get; set; }
        public virtual Image Image { get; set; }
    }
}
