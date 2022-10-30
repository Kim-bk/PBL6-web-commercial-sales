using CommercialClothes.Models.DTOs.Responses.Base;
using System.Collections.Generic;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class UserGroupResponse : GeneralResponse
    {
        public List<UserGroup> UserGroups { get; set; }
    }
}
