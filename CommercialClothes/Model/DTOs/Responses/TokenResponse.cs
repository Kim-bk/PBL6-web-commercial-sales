﻿using CommercialClothes.Models.DTOs.Responses.Base;

namespace CommercialClothes.Models.DTOs.Responses
{
    public class TokenResponse : GeneralResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ShopId { get; set; }
        public int Wallet { get; set; }
    }
}