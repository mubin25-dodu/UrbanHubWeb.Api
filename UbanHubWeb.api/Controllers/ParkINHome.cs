using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Cms;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using UrbanHub.DTO;
using UrbanHub.Entities;
using UrbanHub.shared;
using UrbanHubManagement.repo;
using NetTopologySuite.Geometries;

namespace UrbanHubWeb.api.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkINHome(ParkinHome repo) : ControllerBase
    {
        [HttpGet]
        [Route("ParkINHome")]
        public async Task <IActionResult> GetParkingSpots(int page , int offset)
        {
            var result = await repo.GetAllParkingSpaces(page , offset);
            if (!result.Error) {

                var newresult = result.Data.ParkingSpaces.Select(e =>

                   new ParkingSpaceApiDTO {
                       ID = e.ID,
                       Address = e.Address,
                       RentPerHour = e.RentPerHour,
                       Available=e.Available,
                       IsAvailable=e.IsAvailable,
                       Image=e.Image,
                       Description=e.Description,
                       VehicleType=e.VehicleType,
                       OwnerId=e.OwnerId,
                       Distance=e.Distance,
                       lan = e.Location.X,
                       lon = e.Location.Y
                   }).ToList();

                return Ok(newresult);
            }
            return Ok(result);
        }
        
        [HttpGet]
        [Route("NearBy")]
        public async Task<IActionResult> BrowseNearby(double lat, double lng , double rad , int page , int offset)
        {
            var result = await repo.NearBy(rad, lat, lng , page , offset);
            return Ok(result);
        }

        [HttpPost]
        [Route("Search")]
        public async Task<IActionResult> Search(SearchParkingSpace data , int page , int offset)
        {
            ModelState.Remove("DateAndTime");
            if (!ModelState.IsValid)
            {
                return Ok(new { Error = true, Errors = ModelState });
            }

            var result = await repo.Search(data,page, offset);
            return Ok(result);
        }
    }
}
