using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Models;
using ExpenseTracker.Data;
using ExpenseTracker.Utils;


namespace ExpenseTracker.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Register() => View();
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Register(string username, string password, string? email)
        {
            if (_db.Users.Any(u => u.Username == username))
                return View("Error", $"Username '{username}' already exists");

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
            var hash = PasswordUtil.HashPassword(password).Hash;
            var user = _db.Users.FirstOrDefault(u => u.Username == username && u.PasswordHash == hash);

            if (user == null)
                return View("Error", $"Invalid credentials");

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
