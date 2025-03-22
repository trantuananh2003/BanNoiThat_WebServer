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
                var entityProductItem = _mapper.Map<ProductItem>(modelProductItem);

                //Đã tồn tại
                if (listEntityProductItems.Any(x => x.Id == modelProductItem.Id))
                {
                    entityProductItem.Product_Id = request.ProductId;
                    _uow.ProductRepository.UpdateProductItem(entityProductItem);
                }
                //Chưa tồn tại
                else
                { 
                    entityProductItem.Id = Guid.NewGuid().ToString();
                    entityProductItem.Product_Id = request.ProductId;
                    _uow.ProductRepository.AddProductItem(entityProductItem);
                } 

                var modelRequest = listEntityProductItems.Where(x => x.Id == modelProductItem.Id).FirstOrDefault();
                if (modelProductItem != null && modelProductItem.Image != null && modelProductItem.Image.Length > 0)
                {
                    if(modelRequest != null &&!string.IsNullOrEmpty(modelRequest.ImageUrl))
                        await _blobService.DeleteBlob(modelRequest.ImageUrl, StaticDefine.SD_Storage_Containter);
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(modelProductItem.Image.FileName)}";
                    entityProductItem.ImageUrl = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, modelProductItem.Image);
                }
            }

            await _uow.SaveChangeAsync();
            return Unit.Value;
        }
    }
}
