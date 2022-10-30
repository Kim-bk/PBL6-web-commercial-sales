using CommercialClothes.Models.DTOs.Responses.Base;
using System.Collections.Generic;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class CredentialResponse : GeneralResponse
    {
        public List<string> Roles { get; set; }
    }
}
