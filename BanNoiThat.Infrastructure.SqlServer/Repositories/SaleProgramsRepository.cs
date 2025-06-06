using AutoMapper;
using BanNoiThat.Application.Common;
using BanNoiThat.Application.DTOs.BrandDto;
using BanNoiThat.Application.DTOs.CategoryDtos;
using BanNoiThat.Application.DTOs.ProductDtos;
using BanNoiThat.Application.DTOs.SaleProgramDtos;
using BanNoiThat.Application.Interfaces.IRepository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using Microsoft.EntityFrameworkCore;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class SaleProgramsRepository : Repository<SaleProgram>, ISaleProgramsRepository
    {
        private ApplicationDbContext _db;

        public SaleProgramsRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<SaleProgramIncludeProductsDto> GetSaleProgramsWithUniqueProducts()
        {
            // Lấy danh sách SalePrograms cùng các ProductItems và Product
            var sp = await _db.SalePrograms
                .Where(x => x.IsActive && x.Status == StaticDefine.SP_Status_Active)
                .Include(x => x.ProductItems)
                .ThenInclude(pi => pi.Product)
                .FirstOrDefaultAsync();

            Console.WriteLine($"SaleProgram ID: {sp.Id}, Name: {sp.Name}");

            // Lọc danh sách các Product không trùng lặp
            var uniqueProducts = sp.ProductItems
                .Where(pi => pi.Product != null) // Điều kiện lọc Product
                .GroupBy(pi => pi.Product.Id) // Nhóm theo Product ID
                .Select(group => new ProductHomeResponse
                {
                    Id = group.Key, // ID của Product
                    Name = group.First().Product.Name,
                    Slug = group.First().Product.Slug,
                    ThumbnailUrl = group.First().Product.ThumbnailUrl,
                    ThumbnailUrlSecond = group.First().Product.ProductItems.FirstOrDefault().ImageUrl,
                    Price = group.First().Product.ProductItems.Any() ? group.First().Product.ProductItems.Min(x => x.Price) : 0,
                    SalePrice = group.First().Product.ProductItems.Any() ? group.First().Product.ProductItems.Min(x => x.SalePrice) : 0,
                    Keyword = "",
                    Description = "",
                    IsDeleted = group.First().Product.IsDeleted,
                    Brand = group.First().Product.Brand, // Lấy trực tiếp Brand
                    Category = group.First().Product.Category, // Lấy trực tiếp Category
                    IsHaveModel3D = group.First().Product.ProductItems.Any(x => !string.IsNullOrEmpty(x.ModelUrl)) ? true : false,
                })
                .ToList();


            return new SaleProgramIncludeProductsDto()
            {
                Id = sp.Id,
                Name = sp.Name,
                StartDate = sp.StartDate,
                EndDate = sp.EndDate,
                Slug = sp.Slug,
                ProductResponses = uniqueProducts
            };
        }

    }
}
