using Microsoft.AspNetCore.Identity;

namespace MVCCustomIdentitySample.Models
{
    public class UserClaim<TUser> : IdentityUserClaim<int> where TUser : IdentityUser<int>
    {

        //public virtual TUser? User { get; set; }


    }
}
