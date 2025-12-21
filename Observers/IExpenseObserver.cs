using ExpenseTracker.Domain.Entities;

public interface IExpenseObserver
{
    Task UpdateAsync(Expense expense);   
}