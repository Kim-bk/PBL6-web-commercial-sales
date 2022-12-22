using CommercialClothes.Models.DTOs.Responses.Base;
using Model.DTOs;
using System.Collections.Generic;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class UserGroupResponse : GeneralResponse
    {
        public List<UserGroupDTO> UserGroups { get; set; }
    }
}
