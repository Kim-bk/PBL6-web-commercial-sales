using CommercialClothes.Models.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class UserGroup : BaseEntity
    {
        public UserGroup()
        {
            Accounts = new HashSet<Account>();
        }

        public string Name { get; set; }
        public bool IsDeleted {get; set;}
        public virtual ICollection<Account> Accounts { get; set; }
    }
}
