using MailKit.Search;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanHub.Data;
using UrbanHub.Entities;
using UrbanHubManagement.repo;

namespace UrbanHub.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminTransactionsController(AdminTransactions repo) : Controller
    {
        [HttpGet]
        [Route("Admin/Transactions")]
        public async Task<IActionResult> Transactions( string searchTerm)
        {
            var payments = await repo.Get(searchTerm);
            if (!payments.Error)
            {
                ViewBag.Error = true;
                ViewBag.Message = payments.Message;
            }

            return View(payments.Data);
        }

        [HttpGet]
        public async Task<IActionResult> Task(int id ,string task )
        {
            if (id <= 0)
            {
                TempData["Error"] = true;
                TempData["Message"] = "Invalid user ID.";
                return RedirectToAction("Transactions");
            }

            var users = await repo.PrecoessTask(id , task);
            if (!users.Error)
            {
                ViewBag.Error = true;
                ViewBag.Message = users.Message;
            }
            TempData["Error"] = false;
            TempData["Message"] = users.Message;
            return RedirectToAction("Transactions");
        }
    }
}
