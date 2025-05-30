using AutoMapper;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OutService;
using MediatR;namespace BanNoiThat.Application.Service.Products.Commands.UpdatePatchProduct
{
    public class UpdatePutProductCommandHandler(IUnitOfWork uow, IMapper mapper, IBlobService blob) : IRequestHandler<UpdateProductCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;
        private readonly IBlobService _blobService = blob; 

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.updateProductRequest.Slug))
            {
                request.updateProductRequest.Slug = request.updateProductRequest.Name.GenerateSlug();
            }

            var entity = await _uow.ProductRepository.GetAsync(x => x.Id == request.Id);
            _uow.ProductRepository.AttachEntity(entity);

            entity.Brand_Id = request.updateProductRequest.Brand_Id;
            entity.Category_Id = request.updateProductRequest.Category_Id;
            entity.Slug = request.updateProductRequest.Slug;
            entity.Description = request.updateProductRequest.Description;

            if (request.updateProductRequest.Image != null && request.updateProductRequest.Image.Length > 0)
            {
                string fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.updateProductRequest.Image.FileName)}";
                if(!string.IsNullOrEmpty(entity.ThumbnailUrl))
                {
                    await _blobService.DeleteBlob(entity.ThumbnailUrl.Split('/').Last(), StaticDefine.SD_Storage_Containter);
                }
                entity.ThumbnailUrl = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, request.updateProductRequest.Image);
            }

            await _uow.SaveChangeAsync();
        }
    }
}
