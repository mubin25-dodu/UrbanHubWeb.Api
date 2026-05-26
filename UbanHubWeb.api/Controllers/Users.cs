using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UrbanHubManagement.repo;

namespace UrbanHubWeb.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Users(AdminUserManagement repo) : ControllerBase
    {
        [HttpGet]
        [Route("SearchUser/{Searchtearm}")]
        public async Task<IActionResult> Getall(string Searchtearm)
        {
            var result = await repo.Get(Searchtearm);
            return Ok(result);
        }
        [HttpPost]
        [Route("BanUnban/{id}")]
        public async Task<IActionResult> ban(int id)
        {
            var result = await repo.BanUnbanUser(id);
            return Ok(result);
        }
    }
}
