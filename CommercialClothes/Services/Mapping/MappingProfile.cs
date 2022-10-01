using System;
using AutoMapper;
using ComercialClothes.Models;
using CommercialClothes.Models.DTOs;

namespace CommercialClothes.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Item -> ItemDTO
            CreateMap<Item, ItemDTO>();

            // Image -> ImageDTO
            CreateMap<Image, ImageDTO>();

            // Account -> UserDTO
            CreateMap<Account, UserDTO>();
        }
    }
}
