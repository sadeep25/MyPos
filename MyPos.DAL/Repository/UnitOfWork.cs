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

        private GenericRepository<Customer> clientsRepository;


        public GenericRepository<Customer> ClientsRepository
        {
            get
            {
                if (this.clientsRepository == null)
                {
                    this.clientsRepository = new GenericRepository<Customer>(context);
                }
                return clientsRepository;
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
