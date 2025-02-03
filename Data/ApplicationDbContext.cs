using COMP4870Assignment1.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<CustomUser, CustomRole, string> {
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder) {
        base.OnModelCreating(builder);
        SeedUsersRolesArticles seedUsersRoles = new();
        
        builder.Entity<CustomRole>().HasData(seedUsersRoles.Roles);  
        builder.Entity<CustomUser>()
        .HasData(seedUsersRoles.Users); 
        builder.Entity<IdentityUserRole<string>>().HasData(seedUsersRoles.UserRoles);
        builder.Entity<Articles>().HasData(seedUsersRoles.Articles);
    } 
}

