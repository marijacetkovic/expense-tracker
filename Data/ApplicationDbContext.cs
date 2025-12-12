using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;
using ExpenseTracker.Domain.Entities;


namespace ExpenseTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        //registering tables
        public DbSet<User> Users => Set<User>();
        public DbSet<Expense> Expenses => Set<Expense>();
        public DbSet<ExpenseParticipants> ExpenseParticipants => Set<ExpenseParticipants>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        //configure relationship between Expense and User
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExpenseParticipants>()
                .HasKey(ep => new { ep.ExpenseId, ep.UserId });
        }
    }
}
