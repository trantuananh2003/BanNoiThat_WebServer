using BanNoiThat.Application.Common;
using BanNoiThat.Application.Interfaces.Repository;
using Microsoft.AspNetCore.Authorization;

namespace BanNoiThat.API.Extensions.Authorization
{
    public class AppAuthorizationHandler : IAuthorizationHandler
    {
        private readonly IUnitOfWork _uow;

        public AppAuthorizationHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task HandleAsync(AuthorizationHandlerContext context)
        {
            var requirement = context.PendingRequirements.ToList();
            foreach (var requirementItem in requirement)
            {
                if (requirementItem is PermissionRequirement)
                {
                    var roleId = context.User.Claims.FirstOrDefault(x => x.Type == StaticDefine.Claim_User_Role)?.Value;

                    if (await IsPermission(roleId, (PermissionRequirement)requirementItem))
                    {
                        context.Succeed(requirementItem);
                    }
                }
            }
        }

        private async Task<bool> IsPermission(string roleId, PermissionRequirement requirementItem)
        {
            var entityRole = await _uow.RolesRepository.GetAsync(x => x.Id == roleId, includeProperties: "RoleClaims");

            if (entityRole == null)
            {
                return false;
            }

            foreach (var role in entityRole.RoleClaims)
            {
                if (role.ClaimType == requirementItem.ClaimType && role.ClaimValue == requirementItem.ClaimValue)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
