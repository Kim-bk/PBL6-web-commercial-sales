using CommercialClothes.Models.Base;

namespace CommercialClothes.Models.Entities
{
    public partial class Bank : BaseEntity
    {
        public string BankNumber { get; set; }
        public string BankName { get; set; }
        public string UserName { get; set; }
        public int AccountId { get; set; }
        public virtual Account Account { get; set; }
    }
}
