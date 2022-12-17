using CommercialClothes.Models.Base;
using CommercialClothes.Models.Entities;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Account : BaseEntity
    {
        public Account()
        {
            Ordereds = new HashSet<Order>();
            Banks = new HashSet<Bank>();
        }
#nullable enable

        public int? ShopId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PhoneNumber { get; set; }
        public int? Wallet { get; set; } //only admin and shop has this
        public DateTime DateCreated { get; set; }
        public int? UserGroupId { get; set; }
        public bool IsActivated { get; set; }
        public System.Guid ActivationCode { get; set; }
        public System.Guid ResetPasswordCode { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual UserGroup UserGroup { get; set; }
        public virtual ICollection<Order> Ordereds { get; set; }
        public virtual ICollection<Bank> Banks { get; set; }
    }
}
