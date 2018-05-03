using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPos.DAL.Context;
using MyPos.DAL.Entity;

namespace MyPos.DAL.Repository
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly MyPosDbContext context;
        private GenericRepocitory<Customer> customerRepository;
        private GenericRepocitory<Order> orderRepository;
        private GenericRepocitory<Product> productRepository;
        private GenericRepocitory<OrderItem> orderItemRepository;



        public UnitOfWork(DbContext dbContext)
        {
            context = (MyPosDbContext)dbContext;
        }

       


        public GenericRepocitory<Customer> CustomerRepository
        {
            get
            {
                if (this.customerRepository == null)
                {
                    this.customerRepository = new GenericRepocitory<Customer>(context);
                }
                return customerRepository;
            }
        }

        public GenericRepocitory<Order> OrderRepository
        {
            get
            {
                if (this.orderRepository == null)
                {
                    this.orderRepository = new GenericRepocitory<Order>(context);
                }
                return orderRepository;
            }
        }

        public GenericRepocitory<Product> ProductRepository
        {
            get
            {
                if (this.productRepository == null)
                {
                    this.productRepository = new GenericRepocitory<Product>(context);
                }
                return productRepository;
            }
        }

        public GenericRepocitory<OrderItem> OrderItemRepository
        {
            get
            {
                if (this.orderItemRepository == null)
                {
                    this.orderItemRepository = new GenericRepocitory<OrderItem>(context);
                }
                return orderItemRepository;
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
