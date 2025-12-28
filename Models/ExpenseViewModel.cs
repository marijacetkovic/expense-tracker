namespace ExpenseTracker.Models
{
    public class ExpenseViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Amount { get; set; } 

        public DateTime Date { get; set; } 
        public int CreatedByUserId { get; set; } 
        public int ParticipantCount { get; set; } 
    }
}