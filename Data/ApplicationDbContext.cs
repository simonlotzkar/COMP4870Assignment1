using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using COMP4870Assignment1.Models; // Ensure you have the correct namespace

public class ApplicationDbContext : IdentityDbContext<CustomUser, CustomRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Articles> Articles { get; set; } // Add this to include articles

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        SeedUsersRolesArticles seedUsersRoles = new();
        
        builder.Entity<CustomRole>().HasData(seedUsersRoles.Roles);  
        builder.Entity<CustomUser>().HasData(seedUsersRoles.Users); 
        builder.Entity<IdentityUserRole<string>>().HasData(seedUsersRoles.UserRoles);
        builder.Entity<Articles>().HasData(seedUsersRoles.Articles); 
    }
}
