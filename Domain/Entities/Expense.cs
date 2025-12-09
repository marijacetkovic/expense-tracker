using System.ComponentModel.DataAnnotations;
namespace ExpenseTracker.Domain.Entities;
public class Expense
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public int UserId { get; set; }
}