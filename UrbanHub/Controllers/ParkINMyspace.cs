using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UrbanHubManagement.repo;
using UrbanHub.Entities;

namespace UrbanHub.web.Controllers
{
    [Authorize(Roles = "Owner")]
    public class ParkINMySpace (MySpace repo): Controller
    {
        [Route("MySpace")]
        public IActionResult MySpace()
        {
            var result = repo.GetAll();
            if (result.Error == false)
            {
                ViewBag.Error = true;
                ViewBag.Message = result.Message;
                return View();
            }

            return View(result.Data);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var result = repo.Delete(id);
            if (result.Error == false)
            {
                TempData["Error"] = true;
                TempData["Message"] = result.Message;
                return RedirectToAction("MySpace");
            }
            TempData["Error"] = false;
            TempData["Message"] = result.Message;

            return RedirectToAction("MySpace");
        }

       
    }
}

