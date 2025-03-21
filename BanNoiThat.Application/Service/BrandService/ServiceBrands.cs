using AutoMapper;
using BanNoiThat.Application.DTOs.BrandDto;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.BrandService
{
    public class ServiceBrands : IServiceBrands
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        public ServiceBrands(IMapper mapper, IUnitOfWork uof)
        {
            _mapper = mapper;
            _uow = uof;
        }

        //Nếu khong async nó sẽ lỗi return
        public async Task CreateBrandAsync(CreateBrandRequest modelRequest)
        {
            var entityBrand = _mapper.Map<Brand>(modelRequest);
            entityBrand.Id = Guid.NewGuid().ToString();
            await _uow.BrandRepository.CreateAsync(entityBrand);
            await _uow.SaveChangeAsync();
        }

        public async Task<IEnumerable<BrandResponse>> GetAllBrandAsync()
        {
            var listEntity = await _uow.BrandRepository.GetAllAsync();
            var listDto = _mapper.Map<IEnumerable<BrandResponse>>(listEntity);

            return listDto;
        }

        public async Task<BrandResponse> GetBrandAsync(string id)
        {
            var entity = await _uow.BrandRepository.GetAsync(x => x.Id == id);
            var modelReponse = _mapper.Map<BrandResponse>(entity);

            return modelReponse;
        }


        public async Task UpdateBrandAsync(string id, UpdateBrandRequest modelRequest)
        {
            var entity = await _uow.BrandRepository.GetAsync(x => x.Id == id);
            _uow.BrandRepository.AttachEntity(entity);

            entity.Slug = modelRequest.Slug;
            entity.Name = modelRequest.Name;

            await _uow.SaveChangeAsync();
        }
    }
}
