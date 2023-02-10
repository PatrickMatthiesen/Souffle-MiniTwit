﻿using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models;
public class ApplicationUser : IdentityUser
{
    public List<Message> Messages { get; set; }
    public List<ApplicationUser> Follows { get; set; }
}
