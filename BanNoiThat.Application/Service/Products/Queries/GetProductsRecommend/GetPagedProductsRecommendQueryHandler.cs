
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.Repository;
using MediatR;
using BanNoiThat.Application.Service.Products.Queries.GetProductsRecommend;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Application.Common;

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

        public GetPagedProductsRecommendQueryHandler(IUnitOfWork uow) {
            _uow = uow;
        }

        public async Task<PagedList<ProductHomeResponse>> Handle(GetPagedProductsRecommendQuery request, CancellationToken cancellationToken)
        {
            var listEntityPaged = await _uow.ProductRepository.GetPagedListProduct(request.StringSearch, request.PageSize, request.PageCurrent);
            var listEntity = listEntityPaged.Items;
           
            BasedRecommendations recommendSystem = new BasedRecommendations(await HandleKeyword.ReadKeywordFromVocal());

            var tfArray = recommendSystem.ComputeTF(listEntity);
            var idf = recommendSystem.ComputeIDF(listEntity);

            var vectorTFIDF = recommendSystem.ComputeTFIDF(tfArray,idf);

            var listEntityRecommend = recommendSystem.GetContentBasedRecommendations(request.InteractedProductIds, listEntity, vectorTFIDF);
            listEntityPaged.Items = listEntityRecommend;
            return listEntityPaged;
        }
    }
}
