using BanNoiThat.Application.DTOs.ProductDtos;
using BanNoiThat.Application.DTOs;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Service.Products.Queries.GetProductsRecommend;
using BanNoiThat.Application.Service.RecommendSystem;
using BanNoiThat.Domain.Entities;
using System.Text.RegularExpressions;
using System.Text;
using BanNoiThat.Application.Interfaces.Repository;

namespace BanNoiThat.Application.Service.Products
{
    public class ServiceProduct : IServiceProduct
    {
        private IUnitOfWork _uow;
        public ServiceProduct(IUnitOfWork uow) {
            _uow = uow;
        }

        public async Task<PagedList<ProductHomeResponse>> HandleRecommend(GetPagedProductsRecommendQuery request)
        {
            var listProduct = await _uow.ProductRepository
                .GetAllAsync(x => x.ProductItems.Any(x => x.Quantity > 0), includeProperties: "ProductItems,Category,Brand");

            //Tạo vocabulary
            List<string> vocabulary = new List<string>();
            foreach (var entity in listProduct)
            {
                entity.Keyword = HandleSaveKeyWord(entity);
                var keywords = entity.Keyword.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                foreach (var word in keywords)
                {
                    if (!vocabulary.Contains(word))
                    {
                        vocabulary.Add(word);
                    }
                }
            }

            //var recommendSystem = new BasedRecommendations<Product>(await HandleKeyword.ReadKeywordFromVocal());
            var recommendSystem = new BasedRecommendations<Product>(vocabulary);

            var tfArray = recommendSystem.ComputeTF((List<Product>)listProduct);
            var idf = recommendSystem.ComputeIDF((List<Product>)listProduct);

            var vectorTFIDF = recommendSystem.ComputeTFIDF(tfArray, idf);

            var listEntityRecommend = recommendSystem.GetContentBasedRecommendations(request.InteractedProductIds, (List<Product>)listProduct, vectorTFIDF);
            var totalEntity = listEntityRecommend.Count();

            if (request.PageCurrent != 0 && request.PageSize != 0)
            {
                //Query
                listEntityRecommend = listEntityRecommend.Skip((request.PageCurrent - 1) * request.PageSize).Take(request.PageSize).ToList();
            }

            var modelsReponse = listEntityRecommend.Select(
                product => new ProductHomeResponse()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Slug = product.Slug,
                    ThumbnailUrl = product.ThumbnailUrl,
                    ThumbnailUrlSecond = product.ProductItems.FirstOrDefault().ImageUrl,
                    Keyword = product.Keyword,
                    Price = product.ProductItems.Any() ? product.ProductItems.Min(x => x.Price) : 0,
                    SalePrice = product.ProductItems.Any() ? product.ProductItems.Min(x => x.SalePrice) : 0,
                    TotalSoldQuantity = product.ProductItems.Any() ? product.ProductItems.Sum(x => x.SoldQuantity) : 0,
                    IsHaveModel3D = product.ProductItems.Any(x => !string.IsNullOrEmpty(x.ModelUrl)) ? true : false,
                }).ToList();

            var paged = new PagedList<ProductHomeResponse>(modelsReponse, request.PageCurrent, request.PageSize, totalEntity);

            return paged;
        }

        //Xử lý keyword
        private string HandleSaveKeyWord(Product product)
        {
            string keyword;

            var slugs = product.Slug.Split('-');
            var categorys = (product.Category?.Slug ?? "").Split('-');
            var brands = (product.Brand?.Slug ?? "").Split('-');


            keyword = string.Join(" ", slugs);
            keyword += " " + string.Join(" ", categorys);
            keyword += " " + string.Join(" ", brands);

            keyword = RemoveSpecialCharacters(keyword);

            return keyword;
        }

        // Hàm loại bỏ ký tự đặc biệt
        private string RemoveSpecialCharacters(string input)
        {
            // Chuẩn hóa chuỗi sang dạng FormD để tách dấu
            var normalizedString = input.Normalize(NormalizationForm.FormD);

            // Dùng Regex để chỉ giữ lại các ký tự không phải dấu
            var regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string withoutDiacritics = regex.Replace(normalizedString, string.Empty);

            // Loại bỏ các ký tự đặc biệt ngoài chữ cái và số
            return Regex.Replace(withoutDiacritics, @"[^a-zA-Z0-9\s]", string.Empty);
        }
    }
}
