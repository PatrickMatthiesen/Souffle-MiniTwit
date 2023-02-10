using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models;
public class ApplicationUser : IdentityUser
{
    public List<ApplicationUser> follows { get; set; }
}
