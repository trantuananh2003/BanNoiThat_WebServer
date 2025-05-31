using AutoMapper;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.CategoryDtos;
using BanNoiThat.Application.Interfaces.Database;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OutService;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.Database
{
    public class ServiceCategories : IServiceCategories
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private IBlobService _blobService;

        public ServiceCategories(IUnitOfWork uow, IBlobService blobService, IMapper mapper)
        {
            _uow = uow;
            _mapper = mapper;
            _blobService = blobService;
        }

        public async Task CreateCategoryAsync(CreateCategoriesRequest model)
        {
            var entity = _mapper.Map<Category>(model);
            entity.Id = Guid.NewGuid().ToString();

            if (model.CategoryImage != null && model.CategoryImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.CategoryImage.FileName)}";
                entity.CategoryUrlImage = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, model.CategoryImage);
            }

            await _uow.CategoriesRepository.CreateAsync(entity);
            await _uow.SaveChangeAsync();
        }

        public async Task<IEnumerable<CategoryResponse>> GetCategoriesForClientAsync()
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

        public async Task<IEnumerable<CategoryResponse>> GetCategoriesForAdminAsync()
        {
            var listEntity = await _uow.CategoriesRepository.GetAllAsync(includeProperties: "Children");
            var listResponse = new List<CategoryResponse>();

            foreach (var entity in listEntity)
            {
                if (entity.Children.Any() || string.IsNullOrEmpty(entity.Parent_Id))
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

        public async Task DeleteCategoryHardAsync(string id)
        {
            await _uow.CategoriesRepository.DeleteEntityHard(id);
            await _uow.SaveChangeAsync();
        }

        public async Task UpdateCategoryAsync(string id, UpdateCategoryRequest modelRequest)
        {
            var category = await _uow.CategoriesRepository.GetAsync(x => x.Id == id, tracked: true);
            if (string.IsNullOrEmpty(modelRequest.Slug))
            {
                modelRequest.Slug = modelRequest.Name.GenerateSlug();
            }
            category.Name = modelRequest.Name;
            category.Slug = modelRequest.Slug;

            if (modelRequest.FileImage != null && modelRequest.FileImage.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelRequest.FileImage.FileName)}";
                if(!string.IsNullOrEmpty(category.CategoryUrlImage))
                {
                    await _blobService.DeleteBlob(category.CategoryUrlImage.Split('/').Last(), StaticDefine.SD_Storage_Containter);
                }
                category.CategoryUrlImage = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, modelRequest.FileImage);
            }

            await _uow.SaveChangeAsync();
        }
    }
}
