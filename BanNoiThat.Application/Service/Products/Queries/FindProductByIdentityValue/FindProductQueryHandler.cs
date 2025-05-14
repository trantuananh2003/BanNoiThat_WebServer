using AutoMapper;
using BanNoiThat.Application.DTOs.ProductDtos;
using BanNoiThat.Application.Interfaces.Repository;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Queries.FindProduct
{
    public class FindProductQueryHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<FindProductQuery, ProductResponse>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task<ProductResponse> Handle(FindProductQuery request, CancellationToken cancellationToken)
        {
            var entityProduct = await _uow.ProductRepository.GetAsync((x => x.Slug == request.IdentityValue || x.Id == request.IdentityValue), includeProperties: "Category, Brand, ProductItems");
            var entityProductResponse = _mapper.Map<ProductResponse>(entityProduct);

            return entityProductResponse;
        }
    }
}
