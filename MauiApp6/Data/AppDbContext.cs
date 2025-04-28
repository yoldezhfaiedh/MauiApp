using Microsoft.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore;
using System;

namespace MauiApp6.Data
{
    public class RecruitingContext : DbContext
    {
        public DbSet<UserModel> Users { get; set; } // Modification recommandée (pluriel)
        public DbSet<LostItem> LostItem { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=localhost,1434;Database=Report;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True",
                    options => options.EnableRetryOnFailure());
            }
        }
    }
}