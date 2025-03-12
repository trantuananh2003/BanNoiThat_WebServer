using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using AutoMapper;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<PagedList<ProductHomeResponse>> GetPagedListProduct(string stringSearch,int pageSize, int pageCurrent)
        {
            var query = _db.Products.AsQueryable();
            #region Use Method Syntax
            //As No Tracking

            //Condition
            if(!string.IsNullOrEmpty(stringSearch))
            {
                query = query.Where(x => x.Name.Contains(stringSearch)); 
            }

            //OrderBy and Desc
            //query.OrderBy(x => x.CreateAt);
            query = query.OrderBy(x => x.Name);

            //Total
            var totalCount = query.Count();

            //Query
            query = query.Skip((pageCurrent - 1) * pageSize).Take(pageSize);

            //Select
            var resultQuery = query.Include(x => x.ProductItems)
                .Select(
                product => new ProductHomeResponse()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Slug = product.Slug,
                    ThumbnailUrl = product.ThumbnailUrl,
                    Price = product.ProductItems.Any() ? product.ProductItems.Min(x => x.Price) : 0,
                    SalePrice = product.ProductItems.Any() ? product.ProductItems.Min(x => x.SalePrice) : 0,
                });

            var listEntity = await resultQuery.ToListAsync();
            #endregion

            #region Use expression syntax

            #endregion

            return new PagedList<ProductHomeResponse>(listEntity, pageCurrent, pageSize, totalCount);
        }

        public async Task UpdatePatchProduct(string id, JsonPatchDocument<Product> productPatch)
        {
            var product = await _db.Products.Where(o => o.Id == id).FirstOrDefaultAsync();

            if (product != null)
            {
                productPatch.ApplyTo(product);
            }

            _db.SaveChanges();
        }

        //Where khác gì với Find
        public async Task UpsertProductItemsAsync(string productId, IEnumerable<ProductItem> productItems)
        {
            var product = await _db.Products.Where(x => x.Id == productId).Include(x => x.ProductItems).AsNoTracking().FirstOrDefaultAsync();

            if(product != null)
            {
                foreach (var productItem in productItems)
                {
                    if (product.ProductItems.Any(x => x.Id == productItem.Id))
                    {
                        productItem.Product_Id = productId;
                       
                        _db.ProductItems.Update(productItem);
                    }
                    else
                    {
                        productItem.Id = Guid.NewGuid().ToString();
                        productItem.Product_Id = productId;
                        
                        
                        _db.ProductItems.Add(productItem);
                    }
                }
            }

            await _db.SaveChangesAsync();
        }

        public async Task<ProductItem> GetProductItemByIdAsync(string productItemId)
        {
            var productItemEntity = await _db.ProductItems.Where(x => x.Id == productItemId).FirstOrDefaultAsync();

            return productItemEntity;
        }
    }
}
