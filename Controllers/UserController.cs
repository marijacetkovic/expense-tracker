using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Data;
using ExpenseTracker.Services;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserService _userService;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
            _userService = new UserService(db);
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }
        
            if (_userService.UsernameExists(model.Username)){
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return View(model);
            }
            if (_userService.EmailExists(model.Email)){
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return View(model);
            }

            _userService.CreateUser(model.Username, model.Email, model.Password);

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }
            var user = _userService.GetUser(model.Username);
            
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
