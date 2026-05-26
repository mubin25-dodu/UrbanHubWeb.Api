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
    public class UserLogin(Auth repo) : ControllerBase
    {
        [HttpPost]
        [Route("Login")]
        public async Task <IActionResult> IsUser([FromBody] LoginDTO data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    Error = true, errors = ModelState
                });
            }
            
            var result = repo.IsUser(data);

            if (!result.Error)
            {
                
                    var Claim = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, result.Data.Name),
                        new Claim(ClaimTypes.Email,result.Data.Email),
                        new Claim(ClaimTypes.Role,result.Data.Role),
                        new Claim("UserID", result.Data.Uid.ToString()),
                    };
                    var identity = new ClaimsIdentity(Claim, "UrbanAuth");
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("UrbanAuth", principal);
            }
            return Ok(result);

        }
    }

}
