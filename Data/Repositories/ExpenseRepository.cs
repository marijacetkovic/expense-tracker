using ExpenseTracker.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ExpenseTracker.Data.Repositories;

public class ExpenseRepository
{
    private readonly ApplicationDbContext _db;
    public ExpenseRepository(ApplicationDbContext db) => _db = db;

    public async Task AddAsync(Expense expense)
    {
        await _db.Expenses.AddAsync(expense);
    }

    public async Task SaveChangesAsync() => await _db.SaveChangesAsync();

    public async Task<Expense?> GetAsync(Guid id) => await _db.Expenses.FindAsync(id);
    public async Task<List<Expense>> GetExpensesByUserIdAsync(int userId){
    return await _db.Expenses
        .Include(e => e.Participants)
        .ThenInclude(ep => ep.User)
        .Where(e => e.Participants.Any(ep => ep.UserId == userId))
        .OrderByDescending(e => e.Date)
        .ToListAsync();
    }

    public async Task<Expense?> GetExpenseWithParticipantsAsync(int expenseId)
    {
        return await _db.Expenses
            .Include(e => e.Participants)           
                .ThenInclude(p => p.User)          
            .FirstOrDefaultAsync(e => e.Id == expenseId);
    }
    public async Task<Expense?> GetByIdAsync(int id)
    {
        return await _db.Expenses
            .Include(e => e.Participants)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task UpdateAsync(Expense expense)
    {
        _db.Expenses.Update(expense);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Expense expense)
    {
        _db.Expenses.Remove(expense);
        await _db.SaveChangesAsync();
    }
    
}
