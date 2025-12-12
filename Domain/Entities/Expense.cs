using System.ComponentModel.DataAnnotations;
namespace ExpenseTracker.Domain.Entities;
public class Expense
{
    public int Id { get; set; }         
    public string Name { get; set; }   
    public decimal Amount { get; set; } 
    public DateTime Date { get; set; }  

    public List<ExpenseParticipants> Participants { get; set; } = new();
}
