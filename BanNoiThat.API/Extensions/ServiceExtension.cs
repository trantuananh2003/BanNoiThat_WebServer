using BanNoiThat.Application.Interfaces.Database;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Service.BrandService;
using BanNoiThat.Application.Service.CartsService;
using BanNoiThat.Application.Service.Database;
using BanNoiThat.Application.Service.MomoService;
using BanNoiThat.Application.Service.OutService;
using BanNoiThat.Application.Service.PaymentService;

namespace BanNoiThat.API.Extensions
{
    public static class ServiceExtension
    {
        public static void RegisterDIService(this IServiceCollection services) {
            services.AddSingleton<IBlobService, BlobService>();

            services.AddTransient<IServiceCategories, ServiceCategories>();
            services.AddTransient<IServiceBrands, ServiceBrands>();
            services.AddTransient<IServiceCarts, ServiceCarts>();
            services.AddTransient<IServicePayment, ServicePayment>();
            services.AddScoped<IMomoService, MomoService>();

        }
    }
}
