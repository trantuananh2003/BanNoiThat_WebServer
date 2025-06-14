﻿using AutoMapper;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.BrandDto;
using BanNoiThat.Application.DTOs.CartDtos;
using BanNoiThat.Application.DTOs.CategoryDtos;
using BanNoiThat.Application.DTOs.OrderDtos;
using BanNoiThat.Application.DTOs.ProductDtos;
using BanNoiThat.Application.DTOs.SaleProgramDtos;
using BanNoiThat.Application.DTOs.User;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.JsonPatch;

namespace BanNoiThat.API.Mapper
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //Category
            CreateMap<CreateCategoriesRequest, Category>().
                                ForMember(dest => dest.Slug, opt => opt.MapFrom(x => string.IsNullOrEmpty(x.Slug) ? x.Name.GenerateSlug() : x.Slug));
            CreateMap<Category, CategoryResponse>();

            //Brand
            CreateMap<Brand, BrandResponse>();

            //Product Items
            CreateMap<CreateProductItem, ProductItem>();
            CreateMap<ProductItemRequest, ProductItem>();

            CreateMap<ProductItem, ProductItemResponse>();

            //Product
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.ThumbnailUrlSecond, opt => opt.MapFrom(x => x.ProductItems.FirstOrDefault().ImageUrl))
;
            CreateMap<JsonPatchDocument<UpdateProductRequest>, JsonPatchDocument<Product>>();
            CreateMap<UpdateProductRequest, Product>();
            CreateMap<Product, ProductHomeResponse>();

            //Cart Item
            CreateMap<Cart, CartResponse>();
            CreateMap<CartItem, CartItemResponse>()
                .ForMember(dest => dest.NameOption, opt => opt.MapFrom(cartItem => cartItem.ProductItem.NameOption))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(cartItem => cartItem.ProductItem.Product.Name))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(cartItem => cartItem.ProductItem.ImageUrl))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(cartItem => cartItem.ProductItem.Price))
                .ForMember(dest => dest.SalePrice, opt => opt.MapFrom(cartItem => cartItem.ProductItem.SalePrice));

            //Order
            CreateMap<Order, OrderResponse>();
            CreateMap<OrderItem, OrderItemResponse>();

            //User
            CreateMap<InfoUserRequest, User>();
            CreateMap<User, InfoUserResponse>()
                .ForMember(dest => dest.Birthday, opt => opt.MapFrom(x => x.Birthday.ToString("yyyy-MM-dd")));
            CreateMap<User, UnitUserMangeReponse>()
                        .ForMember(dest => dest.RoleName, opt => opt.MapFrom(x => x.Role.Name));

            //SaleProgram
            CreateMap<SaleProgram, SaleProgramResponse>();
        }
    }
}
