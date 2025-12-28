namespace ExpenseTracker.Models;
public class ExpenseDetailsViewModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public DateTime Date { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal UserShare { get; set; }
    public int CreatedByUserId { get; set; }
    public List<ParticipantViewModel> Participants { get; set; }
}

