using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanHub.Data;
using UrbanHub.Entities;
using UrbanHubManagement.repo;

namespace UrbanHub.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminUserManagementController (AdminUserManagement repo) : Controller
    {
        [Route("admin/users/{searchTerm?}")]
        [HttpGet]
        public async Task<IActionResult> Users( string searchTerm)
        {
            var users = await repo.Get(searchTerm);
            if (!users.Error)
            {
                ViewBag.Error = true;
                ViewBag.Message = users.Message;
            }

            return View(users.Data);
        }

        [HttpGet]
        public async Task<IActionResult> BanUnban(int id)
        {
            if (id<=0)
            {
                TempData["Error"] = true;
                TempData["Message"] = "Invalid user ID.";
                return RedirectToAction("Users");
            }

            var users = await repo.BanUnbanUser(id);
            if (!users.Error)
            {
                ViewBag.Error = true;
                ViewBag.Message = users.Message;
            }
            TempData["Error"] = false;
            TempData["Message"] = users.Message;
            return RedirectToAction("Users");
        }
    }
}
