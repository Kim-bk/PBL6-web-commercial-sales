using Microsoft.EntityFrameworkCore;

#nullable disable

namespace CommercialClothes.Models
{
    [Keyless]
    public partial class Credential
    {
        public int RoleId { get; set; }
        public int UserGroupId { get; set; }

        public virtual Role Role { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
