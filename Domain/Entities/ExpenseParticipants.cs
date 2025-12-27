using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Domain.Entities;

public class ExpenseParticipants
{
    public int ExpenseId { get; set; }
    public Expense? Expense { get; set; } 
    public int UserId { get; set; }
    public User? User { get; set; } 
    public decimal ShareAmount { get; set; }
    //for settlements in the future
    public string Status { get; set; } = "Pending";
}