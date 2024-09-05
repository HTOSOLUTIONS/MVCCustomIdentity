using Microsoft.AspNetCore.Identity;

namespace MVCCustomIdentitySample.Models
{
    public class UserLogin<TUser> : IdentityUserLogin<int> where TUser : IdentityUser<int>
    {

        //public virtual TUser? User { get; set; }


    }
}
