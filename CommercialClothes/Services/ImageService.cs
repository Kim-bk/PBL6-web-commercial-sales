using System;
using System.Threading.Tasks;
using ComercialClothes.Models;
using ComercialClothes.Models.DAL;
using ComercialClothes.Models.DAL.Repositories;
using ComercialClothes.Models.DTOs.Requests;
using CommercialClothes.Services;
using CommercialClothes.Services.Base;
using CommercialClothes.Services.Interfaces;

namespace ComercialClothes.Services
{
    public class ImageService : BaseService, IImageService
    {
        private readonly IImageRepository _imageRepository;
        public ImageService(IImageRepository imageRepository ,IUnitOfWork unitOfWork) : base(unitOfWork)
        {
            _imageRepository = imageRepository;
        }
    }
}