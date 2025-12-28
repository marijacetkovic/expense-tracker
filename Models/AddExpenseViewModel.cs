using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExpenseTracker.Strategies;

namespace ExpenseTracker.Models
{
    public class AddExpenseViewModel
    {
        public AddExpenseViewModel() {
        Participants = new List<ParticipantViewModel>();
        }
        
        public string Title { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Date { get; set; }
        public SplitType SplitType { get; set; } 

        public List<ParticipantViewModel> Participants { get; set; }
        public int CreatedByUserId { get; internal set; }
    }

    public class ParticipantViewModel
    {
        public required string Username { get; set; }
        public decimal ShareAmount { get; set; }
        public bool IsCurrentUser { get; set; }

    }
}
