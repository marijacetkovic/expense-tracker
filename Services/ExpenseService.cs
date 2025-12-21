using ExpenseTracker.Data.Repositories;
using ExpenseTracker.Domain.Entities;

public class ExpenseService
{
    private readonly ExpenseRepository _repo;
    private readonly UserRepository _userRepo;
    private readonly IEnumerable<IExpenseObserver> _observers;

    public ExpenseService(ExpenseRepository repo, UserRepository userRepo, IEnumerable<IExpenseObserver> observers)
    {
        _repo = repo;
        _userRepo = userRepo;
        _observers = observers;
    }

    public async Task<bool> AddExpenseAsync(string title, decimal amount, DateTime date, List<string> participantUsernames)
    {
        if (participantUsernames == null || participantUsernames.Count == 0)
            return false;

        var participants = new List<User>();
        foreach (var username in participantUsernames)
        {
            var user = await _userRepo.GetByUsernameAsync(username);
            if (user == null)
                return false; 
            participants.Add(user);
        }

        // Create expense
        var expense = new Expense
        {
            Name = title,
            Amount = amount,
            Date = date
        };

        // Create ExpenseParticipants
        expense.Participants = participants.Select(u => new ExpenseParticipants
        {
            Expense = expense,
            UserId = u.Id,
            ShareAmount = amount / participants.Count
        }).ToList();

        foreach (var observer in _observers){
            expense.Attach(observer);
        }

        await _repo.AddAsync(expense);
        await _repo.SaveChangesAsync();
        await expense.NotifyAsync();

        return true;
    }

    public async Task<List<Expense>> GetAllUserExpensesAsync()
    {
        return await _repo.GetAllExpensesAsync();
    }
}
