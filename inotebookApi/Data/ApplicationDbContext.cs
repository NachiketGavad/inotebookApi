using inotebookApi.Models;
using Microsoft.EntityFrameworkCore;

namespace inotebookApi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Note>()
                .HasOne<User>()
                .WithMany()
                .HasForeignKey(n => n.UserId)
        .OnDelete(DeleteBehavior.NoAction);  // Specify ON DELETE CASCADE
        }

    }
}
