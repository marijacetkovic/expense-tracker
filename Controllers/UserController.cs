using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Data;
using ExpenseTracker.Services;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }
        
            if (await _userService.CreateUserAsync(model.Username, model.Email, model.Password) == false){
                ModelState.AddModelError(string.Empty, "Username or Email already exists.");
                return View(model);
            }

            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var user = await _userService.GetUser(model.Username);
            
            if (user==null){
                ModelState.AddModelError(string.Empty, "User is not registered.");
                return View(model);
            }

            if (!PasswordService.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt)){
                ModelState.AddModelError(string.Empty, "Incorrect password.");
                return View(model);
            }
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("Role", user.Role);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
