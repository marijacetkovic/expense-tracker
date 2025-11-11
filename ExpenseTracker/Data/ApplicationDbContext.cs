using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Models;


namespace ExpenseTracker.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users => Set<User>();

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
