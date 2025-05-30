using AutoMapper;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Application.Service.OutService;
using BanNoiThat.Domain.Entities;
using MediatR;
using System.Text.RegularExpressions;
using System.Text;
using BanNoiThat.Application.Service.RecommendSystem;

namespace BanNoiThat.Application.Service.Products.Commands.CreateProduct
{
    public class CreateProductsCommandHandler(IUnitOfWork uow, IMapper mapper, IBlobService blobService) : IRequestHandler<CreateProductsCommand, Unit>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;
        private readonly IBlobService _blobService = blobService;

        public async Task<Unit> Handle(CreateProductsCommand request, CancellationToken cancellationToken)
        {
            if(string.IsNullOrEmpty(request.Slug))
            {
                request.Slug = request.Name.GenerateSlug();
            }

            var entityProduct = new Product();
            entityProduct.Id = Guid.NewGuid().ToString();
            entityProduct.Name = request.Name;
            entityProduct.Description = request.Description;
            entityProduct.Category_Id = request.Category_Id;
            entityProduct.Slug = request.Slug;
            entityProduct.Brand_Id = request.Brand_Id;
            entityProduct.ProductItems = new();
            entityProduct.Keyword = HandleSaveKeyWord(entityProduct);
            entityProduct.CreateAt = DateTime.Now;
            HandleKeyword.AddKeyWord(entityProduct.Keyword);

            if (request.ThumbnailImage != null && request.ThumbnailImage.Length > 0)
            {
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(request.ThumbnailImage.FileName)}";
                entityProduct.ThumbnailUrl = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, request.ThumbnailImage);
            }

            if(request.ListProductItems is not null)
            {
                foreach (var productItem in request.ListProductItems)
                {
                    var entityProductItem = _mapper.Map<ProductItem>(productItem);
                    entityProductItem.Id = Guid.NewGuid().ToString();
                    entityProductItem.Product_Id = entityProduct.Id;
                    entityProduct.ProductItems.Add(entityProductItem);

                    if (productItem.Image != null && productItem.Image.Length > 0)
                    {
                        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(productItem.Image.FileName)}";
                        entityProductItem.ImageUrl = await _blobService.UploadBlob(fileName, StaticDefine.SD_Storage_Containter, productItem.Image);
                    }
                }
            }

            await _uow.ProductRepository.CreateAsync(entityProduct);
            await _uow.SaveChangeAsync();

            return Unit.Value;
        }

        private string HandleSaveKeyWord(Product product)
        {
            string keyword;
    
            var names = product.Name.Split(' ');
            var slugs = product.Slug.Split('-');

            keyword = string.Join(" ", names);
            keyword += string.Join(" ", slugs);

            keyword = RemoveSpecialCharacters(keyword);

            return keyword;
        }

        // Hàm loại bỏ ký tự đặc biệt
        private string RemoveSpecialCharacters(string input)
        {
            // Chuẩn hóa chuỗi sang dạng FormD để tách dấu
            var normalizedString = input.Normalize(NormalizationForm.FormD);

            // Dùng Regex để chỉ giữ lại các ký tự không phải dấu
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string withoutDiacritics = regex.Replace(normalizedString, string.Empty);

            // Loại bỏ các ký tự đặc biệt ngoài chữ cái và số
            return Regex.Replace(withoutDiacritics, @"[^a-zA-Z0-9\s]", string.Empty);
        }
    }
}
