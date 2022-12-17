using System.ComponentModel.DataAnnotations;

namespace CommercialClothes.Models.Base
{
    public abstract class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
