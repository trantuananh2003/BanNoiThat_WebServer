using BanNoiThat.Application.DTOs.ProductDtos;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Commands.UpdatePatchProduct
{
    public class UpdateProductCommand : IRequest
    {
        public string Id { get; set; }
        public UpdateProductRequest updateProductRequest { get; set; } 
    }

    //public class UpdatePatchProductDto
    //{
    //    public string Name { get; set; }
    //    public string Category_Id { get; set; }
    //    public string Brand_Id { get; set; }
    //    public string Description { get; set; } = string.Empty;
    //    public string ThumbnailUrl { get; set; } = string.Empty;
    //    public string Slug { get; set; } = string.Empty;
    //}
}
