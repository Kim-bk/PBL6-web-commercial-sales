using System;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Models.DAL;
using CommercialClothes.Models.DAL.Repositories;
using CommercialClothes.Services;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services
{
    public class ImageService : BaseService, IImageService
    {
        private readonly IImageRepository _imageRepository;
        public ImageService(IImageRepository imageRepository, IUnitOfWork unitOfWork
            , IMapperCustom mapper) : base(unitOfWork, mapper)
        {
            _imageRepository = imageRepository;
        }
    }
}