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
}
