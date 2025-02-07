
using Microsoft.AspNetCore.Identity;
using COMP4870Assignment1.Models;

public class SeedUsersRolesArticles
{
    private readonly RoleManager<CustomRole> _roleManager;
    private readonly UserManager<CustomUser> _userManager;
    private readonly ApplicationDbContext _context;

    public SeedUsersRolesArticles(
        RoleManager<CustomRole> roleManager, 
        UserManager<CustomUser> userManager,
        ApplicationDbContext context)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _context = context;
    }

    public async Task SeedDataAsync()
    {
        await SeedRoles();
        await SeedUsers();
        await SeedArticles();
    }

    private async Task SeedRoles()
    {
        var roles = new List<CustomRole>
        {
            new CustomRole("admin", "Role for admin", DateTime.UtcNow),
            new CustomRole("contributor", "Role for contributor", DateTime.UtcNow)
        };

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role.Name))
            {
                await _roleManager.CreateAsync(role);
            }
        }
    }

    private async Task SeedUsers()
    {
        await CreateUser("a@a.a", "P@$$w0rd", "Admin", "User", "admin");
        await CreateUser("c@c.c", "P@$$w0rd", "Contributor", "User", "contributor");
    }

    private async Task CreateUser(string email, string password, string firstName, string lastName, string role)
    {
        if (await _userManager.FindByEmailAsync(email) == null)
        {
            var user = new CustomUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true,
                FirstName = firstName,
                LastName = lastName
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, role);
            }
        }
    }

    private async Task SeedArticles()
    {
        var contributorUser = await _userManager.FindByEmailAsync("c@c.c");

        if (contributorUser != null && !_context.Articles.Any())
        {
            var articles = new List<Articles>
            {
                new Articles
                {
                    ArticleId = 1,
                    Title = "New fires erupt in southern California",
                    Body = " Five new fires have erupted in southern California. The blazes - named Laguna, Sepulveda, Gibbel, Gilman and Border 2 - flared up on Thursday in the counties of Los Angeles, Riverside, and San Diego, prompting evacuations and emergency responses from local fire departments.",
                    CreateDate = new DateTime(2024, 12, 21),
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                    UserId = contributorUser.Id,
                    Email = contributorUser.Email
                }
            };

            _context.Articles.AddRange(articles);
            await _context.SaveChangesAsync();
        }
    }
}
