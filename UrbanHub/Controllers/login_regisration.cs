using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using UrbanHub.custom_services;
using UrbanHub.Data;
using UrbanHub.DTO;
using UrbanHub.Entities;
using UrbanHub.Models;
using UrbanHubManagement.repo;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace UrbanHub.web.Controllers;


public class login_regisration(Auth repo, UrbanHubDbContext context) : Controller
{

    [AllowAnonymous]

    [Route("Login")]
    public IActionResult login_reg()
    {
        if (User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Index", "Home");
        }

        return View();
    }

    public IActionResult logout()
    {
        HttpContext.SignOutAsync("UrbanAuth");
        return RedirectToAction("login_reg");
    }

    [HttpGet]
    [Route("demologin")]
    [AllowAnonymous]
    public async Task<IActionResult> demologin(string role = "user")
    {
        var data = new LoginDTO();

        if (string.Equals(role, "admin", StringComparison.OrdinalIgnoreCase))
        {
            // Demo Admin Credentials - update values as needed to match database record
            data.Email = "urbanhub@gmail.com";
            data.Password = "Mubin@11";
        }
        else if (string.Equals(role, "owner", StringComparison.OrdinalIgnoreCase))
        {
            // Demo Owner Credentials - update values as needed to match database record
            data.Email = "mubin9516@gmail.com";
            data.Password = "Mubin@11";
        }
        else
        {
            // Demo Customer/User Credentials - update values as needed to match database record
            data.Email = "amimubin9@gmail.com";
            data.Password = "Mubin@11";
        }

        var userExist = repo.IsUser(data);
        if (!userExist.Error && userExist.Data != null)
        {
            var Claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userExist.Data.Name),
                new Claim(ClaimTypes.Email, userExist.Data.Email),
                new Claim(ClaimTypes.Role, userExist.Data.Role),
                new Claim("UserID", userExist.Data.Uid.ToString()),
            };
            var identity = new ClaimsIdentity(Claim, "UrbanAuth");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("UrbanAuth", principal);

            if (string.Equals(userExist.Data.Role, "Admin", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Home", "Admin");
            }
            return RedirectToAction("Index", "Home");
        }

        TempData["ErrorMessage"] = $"Demo user login failed for role '{role}': {userExist.Message}";
        return RedirectToAction("login_reg");
    }

    [HttpPost]
    [Route("api/islogin")]
    public async Task<IActionResult> islogin([FromBody] LoginDTO data)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = false, errors = ModelState });
        }

        var userExist = repo.IsUser(data);
        if (!userExist.Error)
        {
            var Claim = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userExist.Data.Name),
                new Claim(ClaimTypes.Email,userExist.Data.Email),
                new Claim(ClaimTypes.Role,userExist.Data.Role),
                new Claim("UserID", userExist.Data.Uid.ToString()),
            };
            var identity = new ClaimsIdentity(Claim, "UrbanAuth");
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync("UrbanAuth", principal);
        }

        return Json(userExist);
    }


    [HttpPost]
    [Route("api/Reg")]
    public IActionResult RegisterEmail([FromBody] Registration data)
    {
        ModelState.Remove("Login");
        if (!ModelState.IsValid)
        {
            return Ok(new { HasError = true, errors = ModelState });
        }
        var register = repo.Register(data);
        return Ok(register);
    }

    //registration page actions

    [Route("Registration")]
    [HttpGet("Registration")]
    public IActionResult Reg_users(string email, int id)
    {
        if (email == null || id == 0)
        {
            return RedirectToAction("RegisterEmail");
        }
        var check = context.Registrations.Where(u => u.Email == email && u.Rid == id);

        if (!check.Any())
        {
            return RedirectToAction("RegisterEmail");
        }

        ViewBag.email = email;
        ViewBag.name = check.First().Name;


        return View();
    }

    [HttpPost("Registration")]
    public IActionResult Reg_users(UserDTO data, string cpass)
    {
        ModelState.Remove("Role");
        ModelState.Remove("ID");
        if (!ModelState.IsValid)
        {

            // logic??
        }
        if (data.Password != cpass)
        {
            ModelState.AddModelError("Password", "Passwords do not match");
        }
        else
        {
            var result = repo.Save(data);
            if (!result.Error)
            {
                return RedirectToAction("login_reg");
            }
            else
            {
                ViewBag.Status = result.Error;
                ViewBag.Message = result.Message;
            }
        }

        ViewBag.email = data.Email;
        ViewBag.name = data.Name;
        ViewBag.phone = data.Phone;
        ViewBag.address = data.Address;

        return View();
    }

    [HttpGet("api/sendotp")]
    public async Task<IActionResult> SendOtp(LoginDTO data )
    {
        ModelState.Remove("Password");
        if (!ModelState.IsValid)
        {
            return Ok(new { HasError = true, errors = ModelState });
        }
        var result = await repo.SendOtp(data.Email);

        if (result.Error)
        {
            return Ok( new { HasError = true , erors=result.Message});
        }
        return Ok(result);
    }

    [HttpGet("api/Resetpass")]
    public async Task<IActionResult> Resetpass(LoginDTO data , int OTP)
    {
        ModelState.Remove("Email");
        if (!ModelState.IsValid)
        {
            return Ok(new { HasError = true, errors = ModelState });
        }
        var result = await repo.Resetpass( data , OTP);

        if (result.Error)
        {
            return Ok(new { HasError = true, erors = result.Message });
        }
        return Ok(result);
    }

}
