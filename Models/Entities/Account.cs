using System;
using System.Collections.Generic;

#nullable disable

namespace ComercialClothes.Models
{
    public partial class Account
    {
        public Account()
        {
            Ordereds = new HashSet<Ordered>();
        }

        public int Id { get; set; }
        public int ShopId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int UserGroupId { get; set; }

        public virtual Shop Shop { get; set; }
        public virtual UserGroup UserGroup { get; set; }
        public virtual ICollection<Ordered> Ordereds { get; set; }
    }
}
