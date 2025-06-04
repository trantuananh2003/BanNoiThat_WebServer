using BanNoiThat.Application.Common;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;

namespace BanNoiThat.Application.Service.SaleProgramService
{
    public class SaleProgramService : IServiceSalePrograms
    {
        private readonly IUnitOfWork _uow;

        public SaleProgramService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task ApplySaleProgramsToProduct(SaleProgram modelSale)
        {
            List<ProductItem> listProductItems = new List<ProductItem>();
            var listApplyValue = modelSale.ApplyValues
                .Split(',')
                .Select(value => value.Trim())
                .ToList();
            if (modelSale.ApplyType == StaticDefine.Sale_ApplyType_Brand)
            {
                var listProducts = await _uow.ProductRepository.GetAllAsync(x => listApplyValue.Contains(x.Brand.Id), isTracked: true, includeProperties: "ProductItems");
                foreach (var product in listProducts)
                {
                    foreach (var productItem in product.ProductItems)
                    {
                        if (productItem.SaleProgram_Id != modelSale.Id)
                        {
                            throw new Exception($"Sản phẩm {product.Name}");
                        }

                        productItem.SaleProgram_Id = modelSale.Id;
                        productItem.SalePrice = productItem.Price - CalculatePrice(modelSale, productItem.Price);
                    }
                }
            }

           await _uow.SaveChangeAsync();
        }

        public async Task PutBackPrice(string modelSaleProgramId)
        {
            var entitySaleProgram = await _uow.SaleProgramsRepository.GetAsync(x => x.Id == modelSaleProgramId, tracked: true, includeProperties: "ProductItems");
            if(entitySaleProgram is not null)
            {
                foreach (var productItem in entitySaleProgram.ProductItems)
                {
                    productItem.SalePrice = productItem.Price;
                }
            }

          
            await _uow.SaveChangeAsync();
        }

        public async Task GetBackPrice(string modelSaleProgramId)
        {
            var entitySaleProgram = await _uow.SaleProgramsRepository.GetAsync(x => x.Id == modelSaleProgramId, tracked: true, includeProperties: "ProductItems");
            foreach (var productItem in entitySaleProgram.ProductItems)
            {
                productItem.SalePrice = productItem.Price - CalculatePrice(entitySaleProgram, productItem.Price);
            }

            await _uow.SaveChangeAsync();
        }

        private double CalculatePrice(SaleProgram saleProgram, double priceOrigin)
        {
            if (saleProgram.DiscountType == StaticDefine.DiscountType_Percent)
            {
                var result = (saleProgram.DiscountValue / 100) * priceOrigin;
                return result > saleProgram.MaxDiscount ? saleProgram.MaxDiscount : result;
            }
            else if (saleProgram.DiscountType == StaticDefine.DiscountType_FixedAmount)
            {
                return saleProgram.DiscountValue;
            }
            else
            {
                return 0;
            }
        }
    }
}
