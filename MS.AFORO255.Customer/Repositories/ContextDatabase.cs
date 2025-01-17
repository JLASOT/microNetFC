using Microsoft.EntityFrameworkCore;
using MS.AFORO255.Customer.Models;
using System;

namespace MS.AFORO255.Customer.Repositories
{
    public class ContextDatabase : DbContext
    {
        public ContextDatabase(DbContextOptions<ContextDatabase> options) : base(options)
        {
        }
        //public DbSet<Customer> Customer { get; set; }
        public DbSet<Models.Customer> Customers { get; set; }

        internal bool Exists(int customer_Id)
        {
            throw new NotImplementedException();
        }
    }
}
