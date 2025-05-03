using MauiApp6.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiApp6.Data
{
    public class RecruitingContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; } // Convention : nom pluriel pour les DbSet
        public DbSet<LostItem> LostItems{ get; set; } // Convention : nom pluriel pour les DbSet
       
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=localhost,1434;Database=Report1;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True",
                    options => options.EnableRetryOnFailure());
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LostItem>()
                .HasOne(li => li.User)
                .WithMany(u => u.LostItems)
                .HasForeignKey(li => li.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
