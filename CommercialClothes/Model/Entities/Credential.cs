﻿using CommercialClothes.Models.Base;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Credential : BaseEntity
    {
        public int RoleId { get; set; }
        public int UserGroupId { get; set; }
        public bool IsActivated { get; set; }
        public virtual Role Role { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
