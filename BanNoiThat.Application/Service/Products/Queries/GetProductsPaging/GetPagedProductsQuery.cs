using BanNoiThat.Application.DTOs;
using MediatR;
using System.Reflection.Metadata.Ecma335;

namespace BanNoiThat.Application.Service.Products.Queries.GetProductsPaging
{
    public class GetPagedProductsQuery : IRequest<PagedList<ProductHomeResponse>>
    {
        public int PageSize { get; set; }
        public int PageCurrent { get; set; }
        public string? StringSearch { get; set; }
    }
}
