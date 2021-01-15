using CarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Data
{
    public class DefaultContext : DbContext
    {
        public DbSet<Car> Cars { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Rental> Rentals { get; set; }

        public DbSet<Discount> Discounts { get; set; }

        public DbSet<Fine> Fines { get; set; }

        public DbSet<RoadAccidient> RoadAccidients { get; set; }

        public DefaultContext(DbContextOptions<DefaultContext> options)
               : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>().Property(c => c.Brand).HasMaxLength(50);
            modelBuilder.Entity<Car>().Property(c => c.Model).HasMaxLength(50);
            modelBuilder.Entity<Car>().Property(c => c.StateNumber).HasMaxLength(10);
            modelBuilder.Entity<Client>().Property(c => c.FirstName).HasMaxLength(50);
            modelBuilder.Entity<Client>().Property(c => c.SecondName).HasMaxLength(60);
            modelBuilder.Entity<Client>().Property(c => c.LastName).HasMaxLength(60);
            modelBuilder.Entity<Client>().Property(c => c.Passport).HasColumnType("nchar(10)");
            modelBuilder.Entity<Client>().Property(c => c.Phone).HasColumnType("nvarchar(15)");
            modelBuilder.Entity<Fine>().Property(c => c.Description).HasMaxLength(200);
            modelBuilder.Entity<Rental>().Property(c => c.ContractNumber).HasMaxLength(50);
            modelBuilder.Entity<Rental>().Property(c => c.EndDateTime).HasComputedColumnSql("(dateadd(day,[number_of_days],[start_datetime]))");
            base.OnModelCreating(modelBuilder);
        }
    }
}
