using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Data;
using ExpenseTracker.Utils;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _db;

        public UserController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var username = model.Username;
            var password = model.Password;
            var email = model.Email;
        
            if (_db.Users.Any(u => u.Username == username)){
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return View(model);
            }
            if (_db.Users.Any(u => u.Email == email)){
                ModelState.AddModelError(string.Empty, "Email is already registered.");
                return View(model);
            }

            var result = PasswordUtil.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = result.Hash,
                PasswordSalt = result.Salt,
                Email = email
            };

            try{
                _db.Users.Add(user);
                _db.SaveChanges();
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Something went wrong. Please try again.");
                return View(model);
            }


            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid) {
                return View(model);
            }

            var user = _db.Users.FirstOrDefault(u => u.Username == model.Username);

            if (user == null){
                ModelState.AddModelError(string.Empty, "Username does not exist.");
                return View(model);
            }

            if (!PasswordUtil.VerifyPassword(model.Password, user.PasswordHash, user.PasswordSalt)){
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
