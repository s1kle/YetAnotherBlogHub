using BlogHub.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlogHub.Identity.Data;

public class AuthorizationDbContext : IdentityDbContext<ApplicationUser>
{
    public AuthorizationDbContext(DbContextOptions<AuthorizationDbContext> options)
        : base(options) { }
}