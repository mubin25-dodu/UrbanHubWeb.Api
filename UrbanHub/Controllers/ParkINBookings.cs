using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanHubManagement.repo;

namespace UrbanHub.web.Controllers
{
    [Authorize]

    public class ParkInBookings(UserBookings repo) : Controller
    {
        public IActionResult MyBookings()
        {
            var result = repo.GetAll();
            return View(result.Data);
        }
        [HttpGet]
        public IActionResult Cancel(int id)
        {
            var result = repo.CancelBooking(id);

                if (!result.Error)
                {
                    TempData["Error"] = true;
                    TempData["Message"] = result.Message;
                }
                else
                {
                    TempData["Error"] = false;
                    TempData["Message"] = result.Message;
                }
            
            return RedirectToAction("MyBookings");
        }
    }
}

