﻿using BanNoiThat.Application.DTOs.ProductDtos;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace BanNoiThat.Application.Service.Products.Commands.CreateProduct
{
    //Coi như là 1 DTO
    public class CreateProductsCommand : IRequest<Unit>
    {
        public string Name { get; set; }
        public string Category_Id { get; set; }
        public string Brand_Id { get; set; }
        public string Description { get; set; }
        public string? Slug { get; set; }
        public IFormFile? ThumbnailImage { get; set; }
        public List<CreateProductItem>? ListProductItems { get; set; }
    }
}
