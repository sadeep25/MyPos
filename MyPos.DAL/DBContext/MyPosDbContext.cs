using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using MyPos.DAL.Entity;
namespace MyPos.DAL.Context
{
    public class MyPosDbContext : DbContext
    {

        public MyPosDbContext() : base("name=DefaultConnection")
        {
            
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
