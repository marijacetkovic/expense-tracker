using ExpenseTracker.Data.Repositories;
using ExpenseTracker.Domain.Entities;
using ExpenseTracker.Models;
using ExpenseTracker.Strategies;

public class ExpenseService
{
    private readonly ExpenseRepository _repo;
    private readonly UserRepository _userRepo;

    public ExpenseService(ExpenseRepository repo, UserRepository userRepo)
    {
        _repo = repo;
        _userRepo = userRepo;
    }

    public async Task<bool> AddExpenseAsync(AddExpenseViewModel model, ISplitStrategy strategy, int creatorId)
    {
        if (model.Participants == null || model.Participants.Count == 0)
            return false;

        var participants = new List<User>();
        foreach (var participant in model.Participants)
        {
            var user = await _userRepo.GetByUsernameAsync(participant.Username);
            if (user == null)
                return false; 
            participants.Add(user);
        }
        
        var calculatedShares = strategy.CalculateShares(model.Amount, participants.Count);
        // Create expense
        var expense = new Expense
        {
            Name = model.Title,
            Date = model.Date,
            CreatedByUserId = creatorId
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

    public async Task<Expense?> GetExpenseWithParticipantsAsync(int id)
    {
        return await _repo.GetExpenseWithParticipantsAsync(id);
    }
    public async Task<List<Expense>> GetAllUserExpensesAsync(int userId)
    {
        return await _repo.GetExpensesByUserIdAsync(userId);
    }   



    public async Task<bool> DeleteExpenseAsync(int expenseId, int userId)
    {
        var expense = await _repo.GetByIdAsync(expenseId);

        // only the creator can delete
        if (expense == null || expense.CreatedByUserId != userId) return false;

        await _repo.DeleteAsync(expense);
        await _repo.SaveChangesAsync();
        return true;
    }
}
