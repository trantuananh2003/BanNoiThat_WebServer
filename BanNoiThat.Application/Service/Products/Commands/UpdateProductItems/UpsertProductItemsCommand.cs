using BanNoiThat.Application.DTOs;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Commands.UpdateProductItems
{
    public class UpsertProductItemsCommand : IRequest<Unit>
    {
        public string ProductId { get; set; }
        public IEnumerable<ProductItemRequest> ListProductItems { get; set; }

    }
}
