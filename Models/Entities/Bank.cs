using CommercialClothes.Models.Base;
using Org.BouncyCastle.Utilities;
using System.Collections.Generic;

#nullable disable
namespace CommercialClothes.Models.Entities
{
    public partial class Bank : BaseEntity
    {
        public string BankNumber { get; set; }
        public string AccountName { get; set; }
        public int AccountId { get; set; }
        public int BankTypeId { get; set; }
        public string? StartedDate { get; set; }
        public string? ExpiredDate { get; set; }
        public virtual Account Account { get; set; }
        public virtual BankType BankType { get; set; }
    }
}

