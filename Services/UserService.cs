using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _db;

        public UserService(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool UsernameExists(string username)
        {
            return _db.Users.Any(u => u.Username == username);
        }

        public bool EmailExists(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }

        public bool CreateUser(string username, string email, string password)
        {
            var result = PasswordService.HashPassword(password);

            var user = new User
            {
                Username = username,
                PasswordHash = result.Hash,
                PasswordSalt = result.Salt,
                Email = email
            };
            
            _db.Users.Add(user);
            _db.SaveChanges();
            return true;
        }

        public User GetUser(string username)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == username);
            return user;
        }
    }
}