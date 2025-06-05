using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using BanNoiThat.Infrastructure.SqlServer.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.JsonPatch;
using BanNoiThat.Application.DTOs.ProductDtos;
using BanNoiThat.Application.Service.Products.Queries.GetProductsPaging;
using System.Linq.Expressions;
using LinqKit;

namespace BanNoiThat.Infrastructure.SqlServer.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _db = context;
        }

        public async Task<PagedList<ProductHomeResponse>> GetPagedListProduct(string stringSearch,int pageSize, int pageCurrent, Boolean IsDeleted, 
            List<PriceRange> priceRanges, List<string> colors,SizeProductItem size)
        {
            var query = _db.Products.AsQueryable();
            #region Use Method Syntax
            //As No Tracking

            //Condition
            if(!string.IsNullOrEmpty(stringSearch))
            {
                var categoryEntity = await _db.Categories.AsNoTracking().Where(c => c.Slug == stringSearch).Include(x => x.Children).FirstOrDefaultAsync();
                var brandEntity = await _db.Brands.AsNoTracking().Where(b => b.Slug == stringSearch).FirstOrDefaultAsync();
                var saleProgram = await _db.SalePrograms.AsNoTracking().Where(sp => sp.Slug == stringSearch).FirstOrDefaultAsync();

                if (categoryEntity != null)
                {
                    if (categoryEntity.Parent_Id == null)
                    {
                        var categoryIds = categoryEntity.Children.Select(c => c.Id).ToList();
                        query = query.Where(x => categoryIds.Contains(x.Category_Id));
                    }
                    else
                    {
                        query = query.Where(x => x.Category_Id == categoryEntity.Id);
                    }
                }
                else if(brandEntity != null)
                {
                    query = query.Where(x => x.Brand_Id == brandEntity.Id);
                }
                else if(saleProgram != null)
                {
                    query = query.Where(x => x.ProductItems.Any(item => item.SaleProgram_Id == saleProgram.Id));
                }
                else
                {
                    query = query.Where(x => x.Name.Contains(stringSearch));
                }

            }
            //Filter delete
            query = query.Where(x => x.IsDeleted == IsDeleted);

            query = query.Include(x => x.Brand).Include(x => x.Category);

            if (priceRanges != null)
            {
                Expression<Func<Product, bool>> expression = x => false; // Khởi tạo với false
                foreach (var priceRange in priceRanges)
                {
                    var minPrice = priceRange.MinPrice;
                    var maxPrice = priceRange.MaxPrice;

                    expression = expression.Or(x => x.ProductItems.Any(item => item.SalePrice >= minPrice && item.SalePrice <= maxPrice));
                }
                query = query.Where(expression);
            }

            if (colors != null)
            {
                Expression<Func<Product, bool>> expression = x => false; // Khởi tạo với false
                foreach (var color in colors)
                {
                    expression = expression.Or(x => x.ProductItems.Any(item => item.Colors.Contains(color)));
                }
                query = query.Where(expression);
            }

            if (size != null)
            {
                if (size.heightSize != null)
                {
                    query = query.Where(x => x.ProductItems.Any(x => x.HeightSize <= size.heightSize));
                }
                if (size.widthSize != null)
                {
                    query = query.Where(x => x.ProductItems.Any(x => x.WidthSize <= size.widthSize));
                }
                if (size.lengthSize != null)
                {
                    query = query.Where(x => x.ProductItems.Any(x => x.LengthSize <= size.lengthSize));
                }
            }

            //OrderBy and Desc
            //query.OrderBy(x => x.CreateAt);
            query = query.OrderByDescending(x => x.CreateAt);

            //Total
            var totalCount = query.Count();

            if (pageSize != 0 && pageCurrent != 0)
            {
                //Query
                query = query.Skip((pageCurrent - 1) * pageSize).Take(pageSize);
            }

            //Select
            var resultQuery = query.Include(x => x.ProductItems)
                .Select(
                product => new ProductHomeResponse()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Slug = product.Slug,
                    ThumbnailUrl = product.ThumbnailUrl,
                    ThumbnailUrlSecond = product.ProductItems.FirstOrDefault().ImageUrl,
                    Keyword = product.Keyword,
                    Description = product.Description,
                    Price = product.ProductItems.Any() ? product.ProductItems.Min(x => x.Price) : 0,
                    SalePrice = product.ProductItems.Any() ? product.ProductItems.Min(x => x.SalePrice) : 0,
                    Brand = product.Brand,
                    Category = product.Category,
                    IsDeleted = product.IsDeleted,
                    IsHaveModel3D = product.ProductItems.Any(x => !string.IsNullOrEmpty(x.ModelUrl)) ? true : false,
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

        public void UpdateProductItem(ProductItem productItem)
        {
            _db.ProductItems.Update(productItem);
        }

        public void AddProductItem(ProductItem productItem)
        {
            _db.ProductItems.Add(productItem);
        }

        public void DeleteProductItem(ProductItem productItem)
        {
            _db.ProductItems.Remove(productItem);
        }

        public async Task<ProductItem> GetProductItemByIdAsync(string productItemId)
        {
            var productItemEntity = await _db.ProductItems.Where(x => x.Id == productItemId).Include(x => x.Product).FirstOrDefaultAsync();

            return productItemEntity;
        }

        public async Task<List<ProductItem>> GetListProductItemByProductIdAsync(string productId)
        {
            var varListProductItems = await _db.ProductItems.Where(x => x.Product_Id == productId).AsNoTracking().ToListAsync();

            return varListProductItems;
        }

        public async Task DeleteSoft(string id,Boolean isDeleted = true)
        {
            var entity = await _dbSet.FindAsync(id);
            if (isDeleted)
            {
                if (entity != null)
                    entity.IsDeleted = true;
            }
            else
            {
                if (entity != null)
                    entity.IsDeleted = false;
            }

        }
    }
}
