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

namespace UrbanHubWeb.api.Controllers

{
    [Route("api/[controller]")]
    [ApiController]
    public class ParkInDetails(ParkinViewDetails repo) : ControllerBase
    {
        [HttpGet]
        [Route("Details/{id}")]
        public async Task <IActionResult> GetParkingDetails(int id)
        {
            var result = await repo.GetParkingSpace(id);
            return Ok(result);
        }
        [Authorize]
        [HttpPost]
        [Route("PlaceBooking")]
        public async Task <IActionResult> Book(ParkingBooking data)
        {
            var result = await repo.RequestBooking(data);
            return Ok(result);
        }
    }

}
