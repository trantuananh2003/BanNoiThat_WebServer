using AutoMapper;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OutService;
using BanNoiThat.Domain.Entities;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Commands.CreateProduct
{
    public class CreateProductsCommandHandler(IUnitOfWork uow, IMapper mapper, IBlobService blobService) : IRequestHandler<CreateProductsCommand, Unit>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;
        private readonly IBlobService _blobService = blobService;

        public async Task<Unit> Handle(CreateProductsCommand request, CancellationToken cancellationToken)
        {
            var entityProduct = new Product();
            entityProduct.Id = Guid.NewGuid().ToString();
            entityProduct.Name = request.Name;
            entityProduct.Description = request.Description;
            entityProduct.Slug = request.Slug;
            entityProduct.Category_Id = request.Category_Id;
            entityProduct.Brand_Id = request.Brand_Id;
            entityProduct.ProductItems = new();

            if (request.ThumbnailImage != null && request.ThumbnailImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ThumbnailImage.FileName)}";
                entityProduct.ThumbnailUrl = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, request.ThumbnailImage);
            }

            foreach (var productItem in request.ListProductItems)
            {
                var entityProductItem = _mapper.Map<ProductItem>(productItem);
                entityProductItem.Id = Guid.NewGuid().ToString();
                entityProductItem.Product_Id = entityProduct.Id;
                entityProduct.ProductItems.Add(entityProductItem);

                if(productItem.Image != null && productItem.Image.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(productItem.Image.FileName)}";
                    entityProductItem.ImageUrl = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, productItem.Image);
                }
            }

            await _uow.ProductRepository.CreateAsync(entityProduct);
            await _uow.SaveChangeAsync();

            return Unit.Value;
        }
    }
}
