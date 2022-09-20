using System;
using System.Collections.Generic;

#nullable disable

namespace ComercialClothes.Models
{
    public partial class Account
    {
        public Account()
        {
            Ordereds = new HashSet<Order>();
        }
#nullable enable
        public int Id { get; set; }
        public int? ShopId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Name { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime DateCreated { get; set; }
        public int? UserGroupId { get; set; }
        public bool IsActivated { get; set; }
        public System.Guid ActivationCode { get; set; }
        public System.Guid ResetPasswordCode { get; set; }
        public virtual Shop Shop { get; set; }
        public virtual UserGroup UserGroup { get; set; }
        public virtual ICollection<Order> Ordereds { get; set; }
    }
}
