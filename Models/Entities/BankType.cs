using CommercialClothes.Models.Base;

namespace CommercialClothes.Models.Entities
{
    public class BankType : BaseEntity
    {
        public string BankName { get; set; }
        public string BankCode { get; set; }
    }
}
