using AutoMapper;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.Database;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.CategoriesService;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.Database
{
    public class ServiceCategories : IServiceCategories
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;

        public ServiceCategories(IUnitOfWork uow, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task CreateCategoryAsync(CreateCategoriesRequest model)
        {
            var entity = _mapper.Map<Category>(model);
            entity.Id = Guid.NewGuid().ToString();
            await _uow.CategoriesRepository.CreateAsync(entity);
            await _uow.SaveChangeAsync();
        }

        public async Task<IEnumerable<CategoryResponse>> GetAllCategoriesAsync()
        {
            var listEntity = await _uow.CategoriesRepository.GetAllAsync(includeProperties: "Children");
            var listResponse = new List<CategoryResponse>();

            foreach(var entity in listEntity)
            {
                if(entity.Children.Any())
                {
                    listResponse.Add(_mapper.Map<CategoryResponse>(entity));
                }
            }

            return listResponse;
        }

        public async Task<CategoryResponse> GetCategoryAsync(string id)
        {
            var entity = await _uow.CategoriesRepository.GetAsync(x => x.Id == id);
            var modelReponse = _mapper.Map<CategoryResponse>(entity);

            return modelReponse;
        }
    }
}
