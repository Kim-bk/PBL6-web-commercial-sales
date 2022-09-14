using System;
using System.Collections.Generic;

#nullable disable

namespace ComercialClothes.Models
{
    public partial class Credential
    {
        public int RoleId { get; set; }
        public int UserGroupId { get; set; }

        public virtual Role Role { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
