using ExpenseTracker.Data.Repositories;
using ExpenseTracker.Domain.Entities;

public class ExpenseService
{
    private readonly ExpenseRepository _repo;
    private readonly UserRepository _userRepo;

    public ExpenseService(ExpenseRepository repo, UserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<bool> AddExpenseAsync(string title, decimal amount, DateTime date, List<string> participantUsernames, ISplitStrategy strategy)
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
        
        var calculatedShares = strategy.CalculateShares(amount, participants.Count);
        // Create expense
        var expense = new Expense
        {
            Name = title,
            Date = date
        };

        // Create ExpenseParticipants
        for (int i = 0; i < participants.Count; i++){
            expense.Participants.Add(new ExpenseParticipants
            {
                UserId = participants[i].Id,
                ShareAmount = calculatedShares[i],
                Status = "Pending"
        });
        }
       
        await _repo.AddAsync(expense);
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<List<Expense>> GetAllUserExpensesAsync()
    {
        return await _repo.GetAllExpensesAsync();
    }
}
