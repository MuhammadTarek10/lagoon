using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Lagoon.Web.Models;
using Lagoon.Application.Services.Interfaces;
using Lagoon.Web.ViewModels;

namespace Lagoon.Web.Controllers;

public class HomeController : Controller
{
    private readonly IVillaService _villaService;

    public HomeController(IVillaService villaService)
    {
        _villaService = villaService;
    }

    public async Task<IActionResult> Index()
    {
        HomeVM homeVM = new HomeVM
        {
            VillaList = await _villaService.GetAllVillasAsync(),
            CheckInDate = DateOnly.FromDateTime(DateTime.Now),
            CheckOutDate = DateOnly.FromDateTime(DateTime.Now.AddDays(1)),
            Nights = 1
        };

        return View(homeVM);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
