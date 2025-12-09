using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using ExpenseTracker.Domain.Entities;


namespace ExpenseTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Expense> Expenses => Set<Expense>();


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
