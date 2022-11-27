using CommercialClothes.Models.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Shop : BaseEntity
    {
        public Shop()
        {
            Accounts = new HashSet<Account>();
            Categories = new HashSet<Category>();
            Images = new HashSet<Image>();
            Items = new HashSet<Item>();
        }

        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public string Description{get; set;}
        public virtual ICollection<Item> Items { get; set; }
        public virtual ICollection<Image> Images { get; set; }
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
    }
}
