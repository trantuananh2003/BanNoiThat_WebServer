﻿using BanNoiThat.Application.DTOs.SaleProgramDtos;

namespace BanNoiThat.Application.DTOs.ProductDtos
{
    public class ProductItemResponse
    {
        public string? Id { get; set; }
        public string NameOption { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double SalePrice { get; set; }
        public string Sku { get; set; }
        public string ImageUrl { get; set; }
        public string? ModelUrl { get; set; }
        public int? LengthSize { get; set; }
        public int? WidthSize { get; set; }
        public int? HeightSize { get; set; }
        public int? Weight { get; set; }
        public string? Colors { get; set; }
        public int SoldQuantity { get; set; }
        public SaleProgramResponse? SaleProgram { get; set; }
    }
}
