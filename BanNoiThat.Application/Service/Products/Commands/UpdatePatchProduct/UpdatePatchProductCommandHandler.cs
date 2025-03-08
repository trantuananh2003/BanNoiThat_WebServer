using AutoMapper;
using Azure;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;

namespace BanNoiThat.Application.Service.Products.Commands.UpdatePatchProduct
{
    public class UpdatePatchProductCommandHandler(IUnitOfWork uow, IMapper mapper) : IRequestHandler<UpdatePatchProductCommand>
    {
        private readonly IUnitOfWork _uow = uow;
        private readonly IMapper _mapper = mapper;

        public async Task Handle(UpdatePatchProductCommand request, CancellationToken cancellationToken)
        {
            var jsonPatchProduct = _mapper.Map<JsonPatchDocument<Product>>(request.JsonPatchProductDto);
            await _uow.ProductRepository.UpdatePatchProduct(request.Id, jsonPatchProduct);
        }
    }
}
