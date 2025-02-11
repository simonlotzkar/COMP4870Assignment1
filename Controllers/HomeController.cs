using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using COMP4870Assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP4870Assignment1.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        // This is used to determine what to display on the index page navbar
        if (User.IsInRole("admin"))
        {
            ViewData["Role"] = "admin";
        }
        else if (User.IsInRole("contributor"))
        {
            ViewData["Role"] = "contributor";
        }
        else
        {
            ViewData["Role"] = "unknown";
        }

        ViewData["FooterText"] = "Kohei Dunnet, Simon Lotzkar and Ben Nguyen";
        // Filter articles to include only those that are not expired (within start and end dates)
        var currentDate = DateTime.Now;
        var articles = _context.Articles
            .Where(a => a.StartDate <= currentDate && a.EndDate >= currentDate) // Check if the article is within the date range
            .Include(a => a.User) // Ensure the User is included
            .ToList();
        // Passing articles to the view so they can be displayed
        ViewData["Articles"] = articles;

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
