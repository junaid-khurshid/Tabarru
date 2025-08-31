using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Tabarru.Common.Enums;
using Tabarru.Common.Helper;
using Tabarru.Services.IServices;

namespace Tabarru.Attributes
{
    public class ValidateKycStatusPolicy : AuthorizationHandler<ValidateKycStatusPolicy>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext validationContext, ValidateKycStatusPolicy validateKycUserStatusPolicy)
        {
            try
            {
                AuthorizationFilterContext authContext = (AuthorizationFilterContext)validationContext.Resource;
                ICharityKycService kycService = (ICharityKycService)authContext.HttpContext.RequestServices.GetService(typeof(ICharityKycService));
                var httpContextAccessor = (IHttpContextAccessor)authContext.HttpContext.RequestServices.GetService(typeof(IHttpContextAccessor));

                var httpContext = (validationContext.Resource as AuthorizationFilterContext)?.HttpContext;
                var claims = httpContext.User;
                var CharityId = TokenClaimHelper.GetId(claims);

                var status = TaskHelper.RunSync(() => kycService.GetCharityKycStatus(CharityId));

                if (status == CharityKycStatus.Approved)
                    //return ValidationResult.Success;
                    validationContext.Succeed(validateKycUserStatusPolicy);
                else
                    validationContext.Fail();
            }
            catch
            {
                validationContext.Fail();
            }
            return Task.FromResult(0);
        }
    }
}
