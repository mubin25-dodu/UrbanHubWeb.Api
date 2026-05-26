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


public class login_regisration(Auth repo , UrbanHubDbContext context) : Controller
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

    [HttpPost]
    [Route("api/islogin")]
    public async Task<IActionResult> islogin([FromBody] LoginDTO data)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new { status = false, errors = ModelState });
        }

        var userExist = repo.UserExist(data);
        if (userExist.Error == true)
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
            return Ok(new { HasError = false, errors = ModelState });
        }
        var register = repo.Register(data);
        if(register.Error)
        {
           int rid = int.Parse(register.AdditionalMessage);
            sendmail(data.Email, data.Name,rid);
        }
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
    public IActionResult Reg_users(User data , string cpass)
    {
        ModelState.Remove("Role");
        ModelState.Remove("ID");
        if (!ModelState.IsValid)
        {
        }
        else if(data.Password!=cpass)
        {
            ModelState.AddModelError("Password", "Passwords do not match");
        }
        else
        {
            var result = repo.Save(data);
            if (result.Error)
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

    private async Task sendmail(string e, string n ,int rid)
    {
        //sending mail
        var email = e;
        var subject = "Registration Confirmation";
        var registrationLink = $"https://localhost:7019/Registration?email={email}&id={rid}";
        var message = $@"
        <!DOCTYPE html>
        <html>
        <head>
            <title>Registration Confirmation</title>
        </head>
        <body style=""font-family: Arial, sans-serif; line-height: 1.6; color: #333;"">
            <div style=""max-width: 600px; margin: 0 auto; padding: 20px; border: 1px solid #ddd; border-radius: 5px;"">
                <h2 style=""color: #007bff;"">Welcome to UrbanHub!</h2>
                <p>Dear {n},</p>
                <p>Thank you for registering with us! We're excited to have you as part of our community.</p>
                <p>Please click the button below to complete your registration and activate your account:</p>
                <p style=""text-align: center;"">
                    <a href='{registrationLink}' style=""background-color: #007bff; color: #ffffff; padding: 10px 20px; text-decoration: none; border-radius: 5px; display: inline-block;"">Complete Registration</a>
                </p>
                <p>If you're having trouble with the button, you can also copy and paste the following link into your browser:</p>
                <p><a href='{registrationLink}'>{registrationLink}</a></p>
                <p>If you did not request this registration, please ignore this email.</p>
                <hr style=""border: 0; border-top: 1px solid #eee;"">
                <p style=""font-size: 0.9em; color: #777;"">Best regards,<br>The UrbanHub Team</p>
            </div>
        </body>
        </html>";

        // Calliing the email sending method
        await new send_email().SendEmail(email, subject, message);
    }
}