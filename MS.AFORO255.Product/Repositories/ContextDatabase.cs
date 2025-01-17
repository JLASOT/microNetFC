using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Product.Models;

namespace MS.AFORO255.Product.Repositories
{
    public class ContextDatabase : DbContext
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options)
        {
        }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Producto> Producto { get; set; }

    }
}
