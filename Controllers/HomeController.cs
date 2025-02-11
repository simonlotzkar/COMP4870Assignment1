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
        var articles = _context.Articles.Include(a => a.User).ToList();

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
