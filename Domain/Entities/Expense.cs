using System.ComponentModel.DataAnnotations;
namespace ExpenseTracker.Domain.Entities;
public class Expense
{
    public int Id { get; set; }         
    public string Name { get; set; }   
    public decimal Amount { get; set; } 
    public DateTime Date { get; set; }  
    
    //navigation property
    public List<ExpenseParticipants> Participants { get; set; } = new();

    //attach observer
    private List<IExpenseObserver> _observers = new();

    public void Attach(IExpenseObserver observer)
    {
        _observers.Add(observer);
    }

    public async Task NotifyAsync()
    {
        foreach (var observer in _observers)
            await observer.UpdateAsync(this);
    }
    
}
