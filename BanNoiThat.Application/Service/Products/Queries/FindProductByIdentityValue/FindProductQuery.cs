using BanNoiThat.Application.DTOs;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Queries.FindProduct
{
    public class FindProductQuery : IRequest<ProductResponse>
    {
        //Slug or Id
        public string IdentityValue { get; set; } = string.Empty;
    }
}
