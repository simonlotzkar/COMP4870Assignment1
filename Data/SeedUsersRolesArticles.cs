
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
            if (!string.IsNullOrEmpty(role.Name))
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    await _roleManager.CreateAsync(role);
                }
            }
        }
    }

    private async Task SeedUsers()
    {
        await CreateUser("a@a.a", "P@$$w0rd", "John", "Doe", "admin");
        await CreateUser("b@b.b", "P@$$w0rd", "Sally", "Ann", "contributor");
        await CreateUser("c@c.c", "P@$$w0rd", "Dylan", "Faye", "contributor");
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
        var contributorUser1 = await _userManager.FindByEmailAsync("c@c.c");
        var contributorUser2 = await _userManager.FindByEmailAsync("b@b.b");

        if (contributorUser1 != null && contributorUser2 != null && !_context.Articles.Any())
        {
            var random = new Random();
            var articles = new List<Articles>
        {
            new Articles
            {
                ArticleId = 1,
                Title = "The Basics of Stock Market Investing for Beginners",
                    Body = "The stock market is where investors buy and sell shares of publicly traded companies. Key players include exchanges like the NYSE and NASDAQ. Beginners should understand stock types (common vs. preferred), risk tolerance, and diversification. Investing can be done through individual stocks, ETFs, or mutual funds. Patience and research are crucial for long-term success.",                CreateDate = new DateTime(2024, 12, 15),
                StartDate = RandomDate(new DateTime(2024, 12, 1), new DateTime(2025, 2, 18), random),
                EndDate = RandomDate(new DateTime(2025, 5, 20), new DateTime(2025, 9, 1), random),
                UserId = contributorUser1.Id,
                Email = contributorUser1.Email
            },
            new Articles
            {
                ArticleId = 2,
                Title = "How to Analyze a Stock: Fundamental vs. Technical Analysis",
                    Body = "Stock analysis is essential for making informed decisions. Fundamental analysis examines financial statements, earnings reports, and company performance. Technical analysis focuses on price movements, trends, and indicators like moving averages. Long-term investors favor fundamentals, while traders rely on technicals for short-term gains.",
                CreateDate = new DateTime(2025, 1, 4),
                StartDate = RandomDate(new DateTime(2024, 12, 1), new DateTime(2025, 2, 18), random),
                EndDate = RandomDate(new DateTime(2025, 5, 20), new DateTime(2025, 9, 1), random),
                UserId = contributorUser2.Id,
                Email = contributorUser2.Email
            },
            new Articles
            {
                ArticleId = 3,
                Title = "The Impact of Economic Indicators on Stock Prices",
                    Body = "Macroeconomic factors influence stock market trends. Inflation and interest rates impact borrowing costs, while GDP growth signals economic health. High unemployment can lower market confidence, while strong job reports boost stock prices. Investors track these indicators to predict market movements.",
                CreateDate = new DateTime(2025, 1, 9),
                StartDate = RandomDate(new DateTime(2025, 2, 19), new DateTime(2025, 4, 15), random),
                EndDate = RandomDate(new DateTime(2025, 4, 16), new DateTime(2025, 5, 18), random),
                UserId = contributorUser1.Id,
                Email = contributorUser1.Email
            },
            new Articles
            {
                ArticleId = 4,
                Title = "Common Stock Market Mistakes and How to Avoid Them",
                    Body = "Many investors make emotional decisions that lead to losses. Avoid panic selling, overtrading, and trying to time the market. Diversify your portfolio, research before investing, and stick to a long-term strategy. Risk management is key to minimizing losses and maximizing gains.",
                CreateDate = new DateTime(2025, 1, 16),
                StartDate = RandomDate(new DateTime(2025, 2, 19), new DateTime(2025, 4, 15), random),
                EndDate = RandomDate(new DateTime(2025, 4, 16), new DateTime(2025, 5, 18), random),
                UserId = contributorUser2.Id,
                Email = contributorUser2.Email
            },
            new Articles
            {
                ArticleId = 5,
                Title = "The Rise of Artificial Intelligence in Stock Trading",
                    Body = "AI is revolutionizing stock trading through algorithmic trading, robo-advisors, and predictive analytics. AI-driven strategies process massive data sets to identify patterns and make real-time decisions. While AI increases efficiency, human oversight is still necessary to manage risks.",
                CreateDate = new DateTime(2025, 1, 19),
                StartDate = RandomDate(new DateTime(2024, 12, 1), new DateTime(2025, 2, 18), random),
                EndDate = RandomDate(new DateTime(2025, 5, 20), new DateTime(2025, 9, 1), random),
                UserId = contributorUser1.Id,
                Email = contributorUser1.Email
            },
            new Articles
            {
                ArticleId = 6,
                Title = "Dividend Stocks vs. Growth Stocks: Which Is Right for You?",
                    Body = "Dividend stocks provide steady income and stability, ideal for conservative investors. Growth stocks reinvest profits to expand, offering high returns but greater risk. Choose dividend stocks for passive income or growth stocks for long-term capital appreciation, depending on your financial goals.",
                CreateDate = new DateTime(2025, 1, 25),
                StartDate = RandomDate(new DateTime(2025, 2, 19), new DateTime(2025, 4, 15), random),
                EndDate = RandomDate(new DateTime(2025, 4, 16), new DateTime(2025, 5, 18), random),
                UserId = contributorUser2.Id,
                Email = contributorUser2.Email
            },
            new Articles
            {
                ArticleId = 7,
                Title = "Understanding Market Crashes: Lessons from History",
                    Body = "Stock market crashes, like 1929, 2008, and 2020, teach valuable lessons. Market downturns are often caused by economic recessions, financial crises, or speculative bubbles. Successful investors stay patient, avoid panic selling, and look for long-term opportunities in downturns.",
                CreateDate = new DateTime(2025, 1, 30),
                StartDate = RandomDate(new DateTime(2024, 12, 1), new DateTime(2025, 2, 18), random),
                EndDate = RandomDate(new DateTime(2025, 5, 20), new DateTime(2025, 9, 1), random),
                UserId = contributorUser1.Id,
                Email = contributorUser1.Email
            }
        };

            _context.Articles.AddRange(articles);
            await _context.SaveChangesAsync();
        }
    }

    private DateTime RandomDate(DateTime start, DateTime end, Random random)
    {
        int range = (end - start).Days;
        return start.AddDays(random.Next(range));
    }

}
