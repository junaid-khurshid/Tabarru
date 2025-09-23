using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Tabarru.Common.Enums;
using Tabarru.Common.Helper;
using Tabarru.Services.IServices;

namespace Tabarru.Attributes
{
    public class ValidateKycStatusPolicy : AuthorizationHandler<ValidateKycStatusPolicy>, IAuthorizationRequirement
    {
        protected override Task HandleRequirementAsync(
     AuthorizationHandlerContext validationContext,
     ValidateKycStatusPolicy requirement)
        {
            try
            {
                HttpContext httpContext = null;

                // Case 1: MVC filter
                if (validationContext.Resource is AuthorizationFilterContext filterContext)
                {
                    httpContext = filterContext.HttpContext;
                }
                // Case 2: Endpoint routing / minimal APIs
                else if (validationContext.Resource is HttpContext ctx)
                {
                    httpContext = ctx;
                }

                if (httpContext == null)
                {
                    validationContext.Fail();
                    return Task.CompletedTask;
                }

                var kycService = (ICharityKycService)httpContext.RequestServices
                    .GetService(typeof(ICharityKycService));

                var claims = httpContext.User;
                var charityId = TokenClaimHelper.GetId(claims);

                var status = TaskHelper.RunSync(() => kycService.GetCharityKycStatus(charityId));

                if (status == CharityKycStatus.Approved)
                {
                    validationContext.Succeed(requirement);
                }
                else
                {
                    validationContext.Fail();
                }
            }
            catch
            {
                validationContext.Fail();
            }

            return Task.CompletedTask;
        }

    }
}
