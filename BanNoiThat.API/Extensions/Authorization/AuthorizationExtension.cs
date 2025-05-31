using BanNoiThat.Application.Common;
using Microsoft.AspNetCore.Authorization;

namespace BanNoiThat.API.Extensions.Authorization
{
    public static class AuthorizationExtension
    {
        public static IServiceCollection SetupAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(
                opts =>
                {
                    opts.AddPolicy("AllowBlockUser", policyBuilder =>
                    {
                        policyBuilder.Requirements.Add(new PermissionRequirement(SDPermissionAccess.Manage, SDPermissionAccess.BlockUser));
                    });
                }
            );

            services.AddTransient<IAuthorizationHandler, AppAuthorizationHandler>();

            return services;
        }
    }
}
