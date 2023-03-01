using Microsoft.AspNetCore.Identity;

namespace MiniTwit.Infrastructure.Models;
public class ApplicationUser : IdentityUser {
    public List<Message> Messages { get; set; }
    public List<ApplicationUser> Follows { get; set; }

    //public ApplicationUser(string name, string email) {
    //    base.UserName = name;
    //    base.Email = email;
    //    Follows = new List<ApplicationUser>();
    //    Messages = new List<Message>();

    //}
}
