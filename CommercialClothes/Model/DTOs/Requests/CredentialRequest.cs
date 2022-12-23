using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommercialClothes.Models.DTOs.Requests
{
    public class CredentialRequest
    {
        public int RoleId { get; set; }
        public int UserGroupId { get; set; }
        public bool IsActive { get; set; }
    }
}
