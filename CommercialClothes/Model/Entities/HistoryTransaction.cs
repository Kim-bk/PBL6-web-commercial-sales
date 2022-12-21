using CommercialClothes.Models;
using CommercialClothes.Models.Base;
using MailKit.Search;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model.Entities
{
    public class HistoryTransaction : BaseEntity
    {
        #nullable disable
        public int? CustomerId { get; set; }
        public int? ShopId { get; set; }
        public int Money { get; set; }
        public DateTime TransactionDate { get; set; }
        public int StatusId { get; set; }
        [ForeignKey(nameof(StatusId))]
        public virtual Status Status { get; set; }
    }
}
