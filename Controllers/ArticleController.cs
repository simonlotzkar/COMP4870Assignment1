using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using COMP4870Assignment1.Models;  

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
        public ActionResult Contributor()
        {
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
    }
}
