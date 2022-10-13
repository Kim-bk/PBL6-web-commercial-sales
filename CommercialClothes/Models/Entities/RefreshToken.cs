using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace CommercialClothes.Models
{
    public partial class RefreshToken
    {
        [Key]
        public string Id { get; set; }
        public string Token { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual Account User { get; set; }
    }
}
