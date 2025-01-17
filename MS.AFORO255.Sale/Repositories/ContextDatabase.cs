using MS.AFORO255.Sale.Models;
using Microsoft.EntityFrameworkCore;

namespace MS.AFORO255.Sale.Repositories
{
    public class ContextDatabase : DbContext
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options)
        {
        }
        //public DbSet<Customer> Customer { get; set; }
        public DbSet<Models.Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SaleDetail>().ToTable("sale_detail")
           .HasOne(sd => sd.Sale)
           .WithMany(s => s.SaleDetails)
           .HasForeignKey(sd => sd.SaleId)
           .OnDelete(DeleteBehavior.Cascade); // Eliminación en cascada

            modelBuilder.Entity<Models.Sale>().ToTable("sale");
            modelBuilder.Entity<SaleDetail>().ToTable("sale_detail");
        }


    }
}
