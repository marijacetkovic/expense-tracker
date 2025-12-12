using ExpenseTracker.Data;
using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

public class UserRepository
{
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<bool> UsernameExistsAsync(string username)
        => await _db.Users.AnyAsync(u => u.Username == username);

    public async Task<bool> EmailExistsAsync(string email)
        => await _db.Users.AnyAsync(u => u.Email == email);

    public async Task AddAsync(User user)
        => await _db.Users.AddAsync(user);

    public async Task SaveChangesAsync()
        => await _db.SaveChangesAsync();

    public async Task<User?> GetByUsernameAsync(string username)
        => await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

    public async Task<List<User>> GetAllAsync()
        => await _db.Users.ToListAsync();
}
