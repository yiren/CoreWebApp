using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BasicsAuthentications.AuthRequirements
{
    // the requirements to pass the authorization middleware
    public class CustomRequirement : IAuthorizationRequirement
    {
        public string ClaimType { get;}
        public CustomRequirement(string claimType)
        {
            this.ClaimType = claimType;

        }
    }
    // how middleware interact with the requirement by implementing Authorization Handler Interface.
    // Every requirement requires the corresponding the handler.
    public class CustomRequirementHandler : AuthorizationHandler<CustomRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CustomRequirement requirement)
        {
            // extract authenticated user info and check if fulfill the requirement
            var hasClaim = context.User.Claims.Any(c => c.Type == requirement.ClaimType);
            if(hasClaim)
            {
                context.Succeed(requirement);
            }
            return Task.CompletedTask;
        }
    }


    public static class AuthorizationPolicyBuilderExtensions
    {
        public static AuthorizationPolicyBuilder RequireCustomClaim(
            this AuthorizationPolicyBuilder builder,
            string claimType
        )
        {
            return builder.AddRequirements(new CustomRequirement(claimType));
        }
    }
}