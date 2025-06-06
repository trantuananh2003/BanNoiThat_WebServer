using BanNoiThat.Application.Common;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Interfaces.Repository;
using BanNoiThat.Domain.Entities;
using Microsoft.Extensions.Logging;
using Quartz;
using System.Text.Json.Serialization;

namespace BanNoiThat.Application.Service.SaleProgramService
{
    [DisallowConcurrentExecution]
    public class SaleProgramService : IServiceSalePrograms, IJob
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<SaleProgramService> _logger;

        public SaleProgramService(IUnitOfWork uow, ILogger<SaleProgramService> logger)
        {
            _uow = uow;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await CheckApplyTime();
        }

        public async Task CheckApplyTime()
        {
            //Chương trình tới hạn nhưng chưa được áp dụng
            var listSalePrograms = await _uow.SaleProgramsRepository.GetAllAsync(x => x.StartDate < DateTime.Now && x.Status == StaticDefine.SP_Status_Inactive);
            _logger.LogWarning("Found {Count} active sale programs", listSalePrograms?.Count() ?? 0);

            foreach (var program in listSalePrograms)
            {
                _logger.LogWarning("Chuong trinh can duoc ap dung: {ProgramName}, Start: {StartDate}, End: {EndDate}",
                    program.Name, program.StartDate, program.EndDate);
                await ApplySaleProgramsToProduct(program.Id);
            }

            //Chương trình hết hạn và chương trình đó đang được áp dụng
            var listSaleProgramsExpried = await _uow.SaleProgramsRepository.GetAllAsync(x => x.EndDate < DateTime.Now && x.Status == StaticDefine.SP_Status_Active);

            foreach (var program in listSaleProgramsExpried)
            {
                _logger.LogWarning("Chuong trinh can duoc tat: {ProgramName}, Start: {StartDate}, End: {EndDate}",
                    program.Name, program.StartDate, program.EndDate);
                await SetExpiredSalePrograms(program.Id);
            }
        }

        public async Task ApplySaleProgramsToProduct(string saleProgramId)
        {
            var entitySP = await _uow.SaleProgramsRepository.GetAsync(x => x.Id == saleProgramId, tracked: true);
            entitySP.Status = StaticDefine.SP_Status_Active;
            List<ProductItem> listProductItems = new List<ProductItem>();
            var listApplyValue = entitySP.ApplyValues
                .Split(',')
                .Select(value => value.Trim())
                .ToList();

            if (entitySP.ApplyType == StaticDefine.Sale_ApplyType_Brand)
            {
                var listProducts = await _uow.ProductRepository.GetAllAsync(x => listApplyValue.Contains(x.Brand.Id), isTracked: true, includeProperties: "ProductItems");
                foreach (var product in listProducts)
                {
                    foreach (var productItem in product.ProductItems)
                    {
                        //Mỗi sản phẩm chỉ được áp dụng vào 1 chương trình
                        //Áp dụng chương trình có giá sale lớn hơn hoặc bằng
                        var priceSaleOfSP = (productItem.Price - CalculatePrice(entitySP, productItem.Price));
                        //Product item khong trong 1 chuong trinh sale
                        if (productItem.SaleProgram_Id != null && productItem.SalePrice > priceSaleOfSP )
                        {
                            continue;
                        }

                        productItem.SaleProgram_Id = entitySP.Id;
                        productItem.SalePrice = productItem.Price - CalculatePrice(entitySP, productItem.Price);
                    }
                }
            }

           await _uow.SaveChangeAsync();
        }

        public async Task SetExpiredSalePrograms(string saleProgramId)
        {
            var entitySP = await _uow.SaleProgramsRepository.GetAsync(x => x.Id == saleProgramId, tracked: true);
            entitySP.Status = StaticDefine.SP_Status_Expired;

            await PutBackPrice(entitySP.Id);

            await _uow.SaveChangeAsync();
        }

        //Đặt lại giá gốc
        public async Task PutBackPrice(string modelSaleProgramId)
        {
            var entitySaleProgram = await _uow.SaleProgramsRepository.GetAsync(x => x.Id == modelSaleProgramId, tracked: true, includeProperties: "ProductItems");
            if(entitySaleProgram is not null)
            {
                foreach (var productItem in entitySaleProgram.ProductItems)
                {
                    productItem.SalePrice = productItem.Price;
                    productItem.SaleProgram_Id = null;
                }
            }

            await _uow.SaveChangeAsync();
        }

        //Lưu lại giá tiền khi có chương trình
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
