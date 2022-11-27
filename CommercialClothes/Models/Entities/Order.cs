using CommercialClothes.Models.Base;
using System;
using System.Collections.Generic;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class Order : BaseEntity
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int AccountId { get; set; }
        public int? StatusId { get; set; }
        public int? PaymentId { get; set; }
        public DateTime DateCreate { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public int ShopId { get; set; }
        public bool IsBought { get; set; }
        public int Total { get; set; }
        public virtual Account Account { get; set; }
        public virtual Payment Payment { get; set; }
        public virtual Status Status { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
