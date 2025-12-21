using ExpenseTracker.Domain.Entities;

public class NotificationObserver : IExpenseObserver
{
    public Task UpdateAsync(Expense expense)
    {
        foreach (var participant in expense.Participants)
        {
            Console.WriteLine($"[Console Notification] Participant {participant.UserId} notified of expense {expense.Name} ({expense.Amount})");
        }
        return Task.CompletedTask;
    }
}

