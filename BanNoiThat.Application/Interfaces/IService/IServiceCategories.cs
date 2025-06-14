﻿using BanNoiThat.Application.DTOs.CategoryDtos;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Interfaces.Database
{
    public interface IServiceCategories
    {
        Task CreateCategoryAsync(CreateCategoriesRequest model);
        Task<CategoryResponse> GetCategoryAsync(string id);
        Task<IEnumerable<CategoryResponse>> GetCategoriesForClientAsync();
        Task<IEnumerable<CategoryResponse>> GetCategoriesForAdminAsync();
        Task DeleteCategoryHardAsync(string id);
        Task UpdateCategoryAsync(string id, UpdateCategoryRequest modelRequest);
    }
}
