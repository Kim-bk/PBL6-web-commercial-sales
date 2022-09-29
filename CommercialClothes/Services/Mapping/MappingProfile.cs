using System;
using AutoMapper;
using ComercialClothes.Models;
using CommercialClothes.Models.DTOs;
using CommercialClothes.Models.DTOs.Responses;

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
        }
    }
}