using System;
using Core.Domian;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {

        }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<ExchageHistory>()
        //        .HasOne<Currency>(s => s.Currency)
        //        .WithMany(g => g.ExchageHistory)
        //        .HasForeignKey(s => s.CurId);

        //}

        public DbSet<Currency> Currencies { get; set; }
        public DbSet<ExchageHistory> ExchageHistory { get; set; }
    }
}
