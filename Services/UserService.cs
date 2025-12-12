using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Services;

public class UserService
{
    private readonly UserRepository _repo;

    public UserService(UserRepository repo)
    {
        _repo = repo;
    }

    public async Task<bool> CreateUserAsync(string username, string email, string password)
    {
        if ((await _repo.UsernameExistsAsync(username)) || (await _repo.EmailExistsAsync(email)))
            return false;

        var result = PasswordService.HashPassword(password);

        var user = new User
        {
            Username = username,
            PasswordHash = result.Hash,
            PasswordSalt = result.Salt,
            Email = email
        };

        await _repo.AddAsync(user);
        await _repo.SaveChangesAsync();
        return true;
    }

    public Task<User?> GetUser(string username) => _repo.GetByUsernameAsync(username);

    public Task<List<User>> GetAllUsers() => _repo.GetAllAsync();
}
