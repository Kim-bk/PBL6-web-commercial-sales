using System;
using System.Collections.Generic;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Shop
    {
        public Shop()
        {
            Accounts = new HashSet<Account>();
            Categories = new HashSet<Category>();
            Images = new HashSet<Image>();
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhomeNumber { get; set; }
        public DateTime DateCreated { get; set; }

        public virtual Payment Payment { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Item> Items { get; set; }
    }
}
