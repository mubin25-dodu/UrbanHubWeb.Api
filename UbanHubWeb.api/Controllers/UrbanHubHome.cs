using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Cms;
using UrbanHub.DTO;
using UrbanHub.Entities;
using UrbanHubManagement.repo;

namespace UrbanHubWeb.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrbanHubHome(PlatformServices repo) : ControllerBase
    {
        [HttpGet]
        [Route("Home")]
        public async Task <IActionResult> GetServices()
        {
            
            var result = await repo.Get();
            return Ok(result);
        }
    }

}
