using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCustomIdentitySample.Models
{
    public class RoleClaim<TRole> : IdentityRoleClaim<int> where TRole : IdentityRole<int>
    {

        //[ForeignKey("RoleId")]
        //public virtual TRole? Role { get; set; }


    }
}
