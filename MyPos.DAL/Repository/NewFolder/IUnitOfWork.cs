using MyPos.DAL.Context;
using MyPos.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.DAL.Repository.NewFolder
{
    public interface IUnitOfWork
    {
         MyPosDbContext DbContext { get; }

        GenericRepository<Customer> CustomerRepository { get; }
        GenericRepository<Order> OrderRepository { get; }
        GenericRepository<Product> ProductRepository { get; }
        GenericRepository<OrderItem> OrderItemRepository { get; }

        void Save();
    }
}
