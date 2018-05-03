using MyPos.DAL.Context;
using MyPos.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.DAL.Repository
{
    public interface IUnitOfWork
    {
         MyPosDbContext DbContext { get; }

        GenericRepocitory<Customer> CustomerRepository { get; }
        GenericRepocitory<Order> OrderRepository { get; }
        GenericRepocitory<Product> ProductRepository { get; }
        GenericRepocitory<OrderItem> OrderItemRepository { get; }

        void Save();
    }
}
