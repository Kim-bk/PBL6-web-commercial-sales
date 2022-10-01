using ComercialClothes.Models.DAL;
using CommercialClothes.Services.Interfaces;

namespace CommercialClothes.Services.Base
{
    public abstract class BaseService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapperCustom _mapper;
        public BaseService(IUnitOfWork unitOfWork, IMapperCustom mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
