using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Data;
using ExpenseTracker.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ExpenseTracker.Controllers
{
    namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        // GET: /User/Register
        public IActionResult Register() => View();

        // GET: /User/Login
        public IActionResult Login() => View();

        // GET: /User/Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return Redirect("/");
        }
    }
}
}
