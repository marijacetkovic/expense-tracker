using System.ComponentModel.DataAnnotations;
namespace ExpenseTracker.Domain.Entities;
public class Expense
{
    public int Id { get; set; }         
    public string Name { get; set; }   
    public DateTime Date { get; set; }  
    public int CreatedByUserId { get; set; }
    
    //navigation property
    public List<ExpenseParticipants> Participants { get; set; } = new();
    //computed property
    public decimal TotalAmount => Participants?.Sum(p => p.ShareAmount) ?? 0;
    
    
}
