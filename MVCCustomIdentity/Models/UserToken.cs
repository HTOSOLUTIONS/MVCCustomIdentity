using Microsoft.AspNetCore.Identity;

namespace MVCCustomIdentitySample.Models
{
    public class UserToken<TUser> : IdentityUserToken<int> where TUser : IdentityUser<int>
    {

        //public virtual TUser? User { get; set; }


    }
}
