using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.Repository;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Queries.GetProductsPaging
{
    public class GetPagedProductsQueryHandler : IRequestHandler<GetPagedProductsQuery, PagedList<ProductHomeResponse>>
    {
        private IUnitOfWork _uow;

        public GetPagedProductsQueryHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<PagedList<ProductHomeResponse>> Handle(GetPagedProductsQuery request, CancellationToken cancellationToken)
        {
            var entityPadeawait = await _uow.ProductRepository.GetPagedListProduct(request.StringSearch, request.PageSize, request.PageCurrent);

            return entityPadeawait;
        }

    }
}
