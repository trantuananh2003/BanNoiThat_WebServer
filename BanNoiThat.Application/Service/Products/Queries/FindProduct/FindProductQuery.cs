using BanNoiThat.Application.DTOs;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Queries.FindProduct
{
    public class FindProductQuery : IRequest<ProductResponse>
    {
        public string Id { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
    }
}
