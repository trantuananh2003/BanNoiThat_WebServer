﻿using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.DTOs.ProductDtos;
using MediatR;

namespace BanNoiThat.Application.Service.Products.Queries.GetProductsPaging
{
    public class GetPagedProductsQuery : IRequest<PagedList<ProductHomeResponse>>
    {
        public int PageSize { get; set; }
        public int PageCurrent { get; set; }
        public string? StringSearch { get; set; }
        public Boolean IsDeleted { get; set; } = false;
        public List<PriceRange>? PriceRanges { get; set; }
        public List<string>? Colors { get; set; }
        public SizeProductItem? Size { get; set; }
    }

    public class PriceRange
    {
        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }
    }

    public class SizeProductItem
    {
        public int? lengthSize {get;set;}
        public int? widthSize { get; set; }
        public int? heightSize { get; set; }
    }
}
