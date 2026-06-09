using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrbanHub.Data;
using UrbanHub.DTO;
using UrbanHub.Entities;
using UrbanHub.shared;
using UrbanHubManagement.repo;

namespace UrbanHub.web.Controllers
{
    public class ParkINHome(ParkinHome repo) : Controller
    {
        [Route("ParkIN")]
        public async Task<IActionResult> Browse(int page)
        {
            if (page == 0)
            {
                page = 1;
            }

            var result = await repo.GetAllParkingSpaces( page , 21);
            return View(result.Data);
        }

        [HttpGet("ParkIN/Nearby")]
        public async Task<IActionResult> BrowseNearby(double lat, double lng)
        {
            var result = await repo.NearBy(5000, lat, lng ,1 ,21);
            return View("Browse", result.Data);
        }

        [HttpPost]
        public async Task<IActionResult> Search(ParkInBrowseModel data)
        {
            if (data.SearchSpaces == null)
            {
                return RedirectToAction("Browse");
            }

            var result = await repo.Search(data.SearchSpaces , 1 , 21);
            return View("Browse", result.Data ?? new ParkInBrowseModel { SearchSpaces = data.SearchSpaces });
        }

        public IActionResult LiveMap()
        {
            return View();
        }


    }

}
