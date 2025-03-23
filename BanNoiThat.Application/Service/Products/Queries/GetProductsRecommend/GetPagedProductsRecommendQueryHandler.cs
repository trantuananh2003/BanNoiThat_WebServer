
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.Repository;
using MediatR;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Application.Service.RecommendSystem;
using AutoMapper;
using System.Text.RegularExpressions;
using System.Text;

namespace BanNoiThat.Application.Service.Products.Queries.GetProductsRecommend
{
    public class TypeValue
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }

    public class GetPagedProductsRecommendQueryHandler : IRequestHandler<GetPagedProductsRecommendQuery, PagedList<ProductHomeResponse>>
    {
        private IUnitOfWork _uow;
        private IMapper _mapper;

        public GetPagedProductsRecommendQueryHandler(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        public async Task<PagedList<ProductHomeResponse>> Handle(GetPagedProductsRecommendQuery request, CancellationToken cancellationToken)
        {
           var listEntity = await _uow.ProductRepository.GetAllAsync(x => x.ProductItems.Any(x => x.Quantity > 0), includeProperties: "ProductItems");

            if (request.PageCurrent == 0)
            {
                foreach(var entity in listEntity)
                {
                    _uow.ProductRepository.AttachEntity(entity);
                    entity.Keyword = HandleSaveKeyWord(entity);
                    HandleKeyword.AddKeyWord(entity.Keyword);
                }
                await _uow.SaveChangeAsync();
            }

            var recommendSystem = new BasedRecommendations<Product>(await HandleKeyword.ReadKeywordFromVocal());

            var tfArray = recommendSystem.ComputeTF((List<Product>)listEntity);
            var idf = recommendSystem.ComputeIDF((List<Product>)listEntity);

            var vectorTFIDF = recommendSystem.ComputeTFIDF(tfArray,idf);

            var listEntityRecommend = recommendSystem.GetContentBasedRecommendations(request.InteractedProductIds, (List<Product>)listEntity, vectorTFIDF);
            var totalEntity = listEntityRecommend.Count();

            if (request.PageCurrent != 0 && request.PageSize != 0)
            {
                //Query
                listEntityRecommend = listEntityRecommend.Skip((request.PageCurrent - 1) * request.PageSize).Take(request.PageSize).ToList();
            }

            var modelsReponse = listEntityRecommend.Select(
                product => new ProductHomeResponse()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Slug = product.Slug,
                    ThumbnailUrl = product.ThumbnailUrl,
                    Keyword = product.Keyword,
                    Price = product.ProductItems.Any() ? product.ProductItems.Min(x => x.Price) : 0,
                    SalePrice = product.ProductItems.Any() ? product.ProductItems.Min(x => x.SalePrice) : 0,
                }).ToList();

            var paged = new PagedList<ProductHomeResponse>(modelsReponse, request.PageCurrent, request.PageSize, totalEntity);

            return paged;
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
