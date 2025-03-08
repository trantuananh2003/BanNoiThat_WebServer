using AutoMapper;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Commands.UpdateProductItems
{
    public class UpsertProductItemsCommandHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpsertProductItemsCommand, Unit>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task<Unit> Handle(UpsertProductItemsCommand request, CancellationToken cancellationToken)
        {
            var listProductItems = _mapper.Map<List<ProductItem>>(request.ListProductItems);
            await _uow.ProductRepository.UpsertProductItemsAsync(request.ProductId, listProductItems);
            return Unit.Value;
        }
    }
}
