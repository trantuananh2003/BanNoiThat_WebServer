using BanNoiThat.Application.DTOs.MailDtos;
using BanNoiThat.Application.Interfaces.Database;
using BanNoiThat.Application.Interfaces.IService;
using BanNoiThat.Application.Service.BrandService;
using BanNoiThat.Application.Service.CartsService;
using BanNoiThat.Application.Service.CouponsService;
using BanNoiThat.Application.Service.Database;
using BanNoiThat.Application.Service.MailsService;
using BanNoiThat.Application.Service.OrderService;
using BanNoiThat.Application.Service.OutService;
using BanNoiThat.Application.Service.PaymentMethod.MomoService;
using BanNoiThat.Application.Service.PaymentMethod.PayVnService;
using BanNoiThat.Application.Service.PaymentService;
using BanNoiThat.Application.Service.SaleProgramService;
using BanNoiThat.Application.Service.StatisticService;
using BanNoiThat.Application.Service.UserService;
using BanNoiThat.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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
            services.AddTransient<IServiceOrder, OrderService>();
            services.AddTransient<IServiceUser, ServiceUser>();
            services.AddScoped<IMomoService, MomoService>();
            services.AddScoped<IVnPayService, VnPayService>();
            services.AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();
            services.AddTransient<IStatisticService, StatisticService>();

            services.AddTransient<IServiceCoupon, CouponService>();
            services.AddTransient<IServiceCouponUsage, CouponUsageService>();

            services.AddTransient<IServiceSalePrograms, SaleProgramService>();
            services.AddHttpClient<ServiceShipping>();
            services.AddTransient<ServiceShipping>();

            services.AddDataProtection();
            services.AddScoped<DataProtectorTokenProviderService>();
        }
        public static void RegisterMailService(this IServiceCollection services, IConfiguration configuration)
        {
            var mailSettings = configuration.GetSection("MailSettings");
            services.Configure<MailSettings>(mailSettings);
            services.AddTransient<SendMailService>();
        }
    }
}
