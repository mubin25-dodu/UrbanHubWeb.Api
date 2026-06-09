using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Construction;
using UrbanHub.DTO;
using UrbanHub.Entities;
using UrbanHub.shared;
using UrbanHubManagement.repo;

namespace UrbanHub.web.Controllers
{
    public class ParkINDetails (ParkinViewDetails repo) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> ViewDetails(int id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }

            var result = await repo.GetParkingSpace(id);
            if (!result.Error)
            {
                var newresult = new ParkingDetailsModel();
                newresult.ParkingSpaces = result.Data.ParkingSpaces;
                newresult.ParkingBookings = result.Data.ParkingBookings;
                return View(newresult);
            }
            return NotFound();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> RequestBooking(ParkingDetailsModel data)
        {
            if (data == null)
            {
                return BadRequest();
            } 
            var result = await repo.RequestBooking(data.ParkingBooking);

            if (!result.Error )
            {
                TempData["Error"] = true;
                TempData["Message"] = result.Message;
            }
            else
            {
                TempData["Error"] = false;
                TempData["Message"] = result.Message;
            }
            
            return RedirectToAction("ViewDetails" , new { id = data.ParkingBookingDTO.ParkingID });
        }

    }
}
