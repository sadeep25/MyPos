using MyPos.DAL.Context;
using MyPos.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.DAL.Repository
{
    public class UnitOfWork : IDisposable
    {
        private MyPosDbContext context = new MyPosDbContext();

        private GenericRepository<Customer> customerRepository;
        private GenericRepository<Order> orderRepository;
        private GenericRepository<Product> productRepository;


        public GenericRepository<Customer> CustomerRepository
        {
            get
            {
                if (this.customerRepository == null)
                {
                    this.customerRepository = new GenericRepository<Customer>(context);
                }
                return customerRepository;
            }
        }
        public GenericRepository<Order> OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new GenericRepository<Order>(context);
                }
                return orderRepository;
            }
        }public GenericRepository<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new GenericRepository<Product>(context);
                }
                return productRepository;
            }
        }
        public MyPosDbContext DbContext
        {
            get { return context; }
        }

        public void Save()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
