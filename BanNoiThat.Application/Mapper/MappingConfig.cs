using AutoMapper;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Service.BrandService;
using BanNoiThat.Application.Service.CategoriesService;
using BanNoiThat.Application.Service.Products.Commands.CreateProduct;
using BanNoiThat.Application.Service.Products.Commands.UpdatePatchProduct;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BanNoiThat.API.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig() {
            //Category
            CreateMap<CreateCategoriesRequest, Category>();
            CreateMap<Category, CategoryResponse>();

            //Brand
            CreateMap<CreateBrandRequest, Brand>();
            CreateMap<Brand, BrandResponse>();

            //Product Items
            CreateMap<CreateProductItem, ProductItem>();
            CreateMap<ProductItemRequest, ProductItem>();

            CreateMap<ProductItem, ProductItemResponse>();

            //Product
            CreateMap<Product, ProductResponse>();
            CreateMap<JsonPatchDocument<UpdatePatchProductDto>, JsonPatchDocument<Product>>();
            CreateMap<UpdatePatchProductDto, Product>();

            //Cart Item
            CreateMap<Cart, CartResponse>();
            CreateMap<CartItem, CartItemResponse>()
                .ForMember(dest => dest.NameOption, opt => opt.MapFrom(cartItem => cartItem.ProductItem.NameOption))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(cartItem => cartItem.ProductItem.Product.Name))
                .ForMember(dest => dest.ThumbnailUrl, opt => opt.MapFrom(cartItem => cartItem.ProductItem.Product.ThumbnailUrl));
        } 
    }
}
