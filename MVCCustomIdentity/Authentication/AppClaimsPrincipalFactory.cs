using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using MVCCustomIdentitySample.Models;

namespace MVCCustomIdentitySample.Authentication
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {


        public AppClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor)
        {

        }

        public async override Task<ClaimsPrincipal> CreateAsync(AppUser user)
        {

            var principal = await base.CreateAsync(user);
            var identity = (ClaimsIdentity)principal.Identity;
            Claim adminrole = new Claim(ClaimTypes.Role, "Administrator");
            identity.AddClaim(adminrole);

            return principal;

        }



    }
}
