using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Domain.Entities;
using System.Threading.Tasks;

namespace ExpenseTracker.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserService _userService;

    public HomeController(ILogger<HomeController> logger, UserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var Users = _userService.GetAllUsers().Result;
        Console.WriteLine("---- Users List ----");
        foreach (var user in Users)
        {
            Console.WriteLine($"ID: {user.Id}, Username: {user.Username}, Email: {user.Email}, Role: {user.Role}");
        }

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
