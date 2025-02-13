using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using COMP4870Assignment1.Models;
using Microsoft.EntityFrameworkCore;

namespace COMP4870Assignment1.Controllers
{
    /**
    * This is the controller class that is used for the both the contributors views 
    * and the admin views. Most likely it should be split up but I left it together 
    * for the purposes of getting it all setup.
    *
    * Have not added any logic for creating an article as a contributor.
    **/
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<CustomRole> _roleManager;

        public ArticleController(ApplicationDbContext context, UserManager<CustomUser> userManager, RoleManager<CustomRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        // View for contributor
        [Authorize(Roles = "contributor")]
        public async Task<ActionResult> Contributor()
        {
            var user = await _userManager.GetUserAsync(User);

            var articles = _context.Articles
                .Where(a => a.UserId == user!.Id)
                .ToList();

            ViewData["Article"] = articles;
            return View();
        }

        // View for admin
        [Authorize(Roles = "admin")]
        public ActionResult Admin()
        {
            ViewData["Users"] = _context.Users.ToList();
            return View();
        }

        // Action for creating a contributor
        [HttpPost]
        public async Task<IActionResult> MakeContributor(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null && !await _userManager.IsInRoleAsync(user, "Contributor"))
            {
                await _userManager.AddToRoleAsync(user, "Contributor");
            }

            return RedirectToAction("Admin");
        }

        // Action for deleting a contributor
        [HttpPost]
        public async Task<IActionResult> DeleteContributor(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Admin");
        }

        // Action for creating an article
        [Authorize(Roles = "contributor")]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "contributor")]
        public async Task<IActionResult> Create(Articles article)
        {
            var user = await _userManager.GetUserAsync(User);

            article.UserId = user!.Id;
            article.CreateDate = DateTime.Now;
            article.Email = user.Email;
            _context.Add(article);
            await _context.SaveChangesAsync();

            return RedirectToAction("Contributor");
        }

        // Action for editing an article
        [Authorize(Roles = "contributor")]
        public async Task<IActionResult> Edit(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (article == null || article.UserId != user!.Id)
            {
                return Unauthorized();
            }

            return View(article);
        }

        [HttpPost]
        [Authorize(Roles = "contributor")]
        public async Task<IActionResult> Edit(Articles article)
        {
            var user = await _userManager.GetUserAsync(User);

            article.Email = user!.Email;
            article.CreateDate = DateTime.Now;

            if (article.UserId != user!.Id)
            {
                return Unauthorized();
            }

            if (ModelState.IsValid)
            {
                _context.Update(article);
                await _context.SaveChangesAsync();
                return RedirectToAction("Contributor");
            }
            return View(article);
        }

        // Action for deleting an article
        [HttpPost]
        [Authorize(Roles = "contributor")]
        public async Task<IActionResult> Delete(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            var user = await _userManager.GetUserAsync(User);

            if (article == null || article.UserId != user!.Id)
            {
                return Unauthorized();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return RedirectToAction("Contributor");
        }


        public IActionResult Details(int id)
        {
            var article = _context.Articles.Include(a => a.User).FirstOrDefault(a => a.ArticleId == id);
            if (article == null)
            {
                return NotFound();
            }
            return View(article);
        }
    }
}
