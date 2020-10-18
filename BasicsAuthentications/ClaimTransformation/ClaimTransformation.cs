using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
namespace BasicsAuthentications.ClaimTransformation
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var hasHelloClaim = principal.Claims.Any(c=> c.Type ==ClaimTypes.MobilePhone);

            if(!hasHelloClaim)
                ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(ClaimTypes.MobilePhone, "0000"));

            return Task.FromResult(principal);
        }
    }
}