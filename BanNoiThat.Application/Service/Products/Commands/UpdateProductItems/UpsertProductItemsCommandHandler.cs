using AutoMapper;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OutService;
using BanNoiThat.Domain.Entities;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Commands.UpdateProductItems
{
    public class UpsertProductItemsCommandHandler(IUnitOfWork uow, IMapper mapper, IBlobService blobService) : IRequestHandler<UpsertProductItemsCommand, Unit>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;
        private IBlobService _blobService = blobService;

        public async Task<Unit> Handle(UpsertProductItemsCommand request, CancellationToken cancellationToken)
        {
            var listEntityProductItems = await _uow.ProductRepository.GetListProductItemByProductIdAsync(request.ProductId);
 
            foreach (var modelProductItem in request.ListProductItems)
            {
                var _entityProductItem = listEntityProductItems.Where(x => x.Id == modelProductItem.Id).FirstOrDefault();
                var entityProductItem = _mapper.Map<ProductItem>(modelProductItem);

                //Đã tồn tại
                if (_entityProductItem != null)
                {
                    if (modelProductItem.IsDelete)
                    {
                        _uow.ProductRepository.DeleteProductItem(entityProductItem);
                    }
                    else
                    {
                        entityProductItem.Product_Id = request.ProductId;
                        if(modelProductItem.ImageProductItem is null)
                        {
                            entityProductItem.ImageUrl = _entityProductItem.ImageUrl;
                        }
                        if(modelProductItem.Model3DProductItem is null)
                        {
                            entityProductItem.ModelUrl = _entityProductItem.ModelUrl;
                        }
                        
                        _uow.ProductRepository.UpdateProductItem(entityProductItem);
                    }
                }
                //Chưa tồn tại
                else
                { 
                    entityProductItem.Id = Guid.NewGuid().ToString();
                    entityProductItem.Product_Id = request.ProductId;
                    _uow.ProductRepository.AddProductItem(entityProductItem);
                } 

                var modelRequest = listEntityProductItems.Where(x => x.Id == modelProductItem.Id).FirstOrDefault();
                if (modelProductItem != null && modelProductItem.ImageProductItem != null && modelProductItem.ImageProductItem.Length > 0)
                {
                    if(modelRequest != null &&!string.IsNullOrEmpty(modelRequest.ImageUrl))
                        await _blobService.DeleteBlob(modelRequest.ImageUrl, StaticDefine.SD_Storage_Containter);
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelProductItem.ImageProductItem.FileName)}";
                    entityProductItem.ImageUrl = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, modelProductItem.ImageProductItem);
                }

                if (modelProductItem != null && modelProductItem.Model3DProductItem != null && modelProductItem.Model3DProductItem.Length > 0)
                {
                    if (modelRequest != null && !string.IsNullOrEmpty(modelRequest.ModelUrl))
                        await _blobService.DeleteBlob(modelRequest.ModelUrl, StaticDefine.SD_Storage_Containter);
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelProductItem.Model3DProductItem.FileName)}";
                    entityProductItem.ModelUrl = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, modelProductItem.Model3DProductItem);
                }
            }

            await _uow.SaveChangeAsync();
            return Unit.Value;
        }
    }
}
