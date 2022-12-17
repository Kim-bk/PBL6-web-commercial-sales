using CommercialClothes.Models.DTOs.Responses.Base;
using System.Collections.Generic;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class CredentialResponse
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public bool IsActivated { get; set; }
    }
}
