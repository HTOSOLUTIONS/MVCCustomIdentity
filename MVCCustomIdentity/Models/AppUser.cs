using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCCustomIdentitySample.Models
{

    public class AppUser : IdentityUser<int>
    {
    }

}
