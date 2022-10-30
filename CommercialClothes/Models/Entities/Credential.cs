using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Credential
    {
        [Key]
        public int Id { get; set; }   
        public int RoleId { get; set; }
        public int UserGroupId { get; set; }
        public virtual Role Role { get; set; }
        public virtual UserGroup UserGroup { get; set; }
    }
}
