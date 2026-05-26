using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using UrbanHub.Data;
using UrbanHub.DTO;
using UrbanHub.Models;
using UrbanHubManagement;
using UrbanHubManagement.repo;

namespace UrbanHub.web.Controllers;

public class HomeController(ILogger<HomeController> logger) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    public IActionResult Index()
    {
        //var data = context.Users.ToList();
        ////var map = _mapper.Map<List<RegistrationDTO>>(data);
        //return Json(context.GetUsers());
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
