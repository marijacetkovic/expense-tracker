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
        public IActionResult Register(string username, string password, string? email)
        {
            if (_db.Users.Any(u => u.Username == username))
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = $"Username '{username}' already exists"
                });


            var result = PasswordUtil.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = result.Hash,
                PasswordSalt = result.Salt,
                Email = email
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            return RedirectToAction("Login");
        }

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);

            if (user == null || !PasswordUtil.VerifyPassword(password, user.PasswordHash, user.PasswordSalt))
                return View("Error", new ErrorViewModel
                {
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    Message = "Invalid credentials"
                });

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
