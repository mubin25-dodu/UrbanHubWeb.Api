using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanHubManagement.repo;

namespace UrbanHub.web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminLogsController (AdminLogs repo) : Controller
    {
        public async Task<IActionResult> Logs()
        {
            var result = await repo.Get();
            return View(result.Data);
        }
    }
}
