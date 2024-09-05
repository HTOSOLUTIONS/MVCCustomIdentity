using Microsoft.AspNetCore.Identity;

namespace MVCCustomIdentitySample.Models
{
    public class UserRole<TUser,TRole> : IdentityUserRole<int> where TUser : IdentityUser<int> where TRole : IdentityRole<int>
    {

        //public virtual TUser? User { get; set; }

        //public virtual TRole? Role { get; set; }

        //public UserRole()
        //{

        //}

        //public UserRole(TUser user, TRole role)
        //{
        //    this.User = user;
        //    this.Role = role;
        //}

    }
}
