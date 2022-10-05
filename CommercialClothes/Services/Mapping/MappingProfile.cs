using CommercialClothes.Models.DTOs.Responses;
using AutoMapper;
using CommercialClothes.Models;
using ComercialClothes.Models.DTOs.Responses;

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

