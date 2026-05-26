using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanHub.Entities;
using UrbanHub.shared;
using UrbanHubManagement.repo;

namespace UrbanHub.web.Controllers
{
    [Authorize]

    public class ParkINPayment(Payment repo) : Controller
    {
        [HttpGet]
        public IActionResult PaymentDetails(int id)
        {
            if (id <= 0 )
            {
                return BadRequest();
            }

            var result = repo.Getbooking(id);
            
            if (!result.Error || result.Data == null)
            {
                return NotFound();
            }
            
            return View(result.Data);
        }       
        
        [HttpGet]
        public IActionResult ProcessPayment(int id)
        {
            if (id <= 0 )
            {
                return BadRequest();
            }

            var result = repo.ProcessPayment(id);
            
            if (result.Error)
            {
                TempData["Message"] = result.Message;
                TempData["Error"] = false;
                return RedirectToAction("MyBookings", "ParkInBookings");
            }
            
            return RedirectToAction("PaymentDetails", new { id = id } );
        }


        
    }
}

