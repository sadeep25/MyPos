using MyPos.BL.Exceptions;
using MyPos.DAL.Entity;
using MyPos.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.BL.Services
{
    public class CustomerService
    {
        private UnitOfWork unitOfWork;

        public CustomerService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Add(Customer model)
        {
            if (model == null) { throw new CustomerNotFoundException(); }

            try
            {
                unitOfWork.CustomerRepository.Insert(model);

                unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    throw new MyPosDbException("Operation Could Not Carry Out Due To An Database Error", ex);
                }
                   
                throw ex;
            }
           
        }

        public Customer GetCustomerByID(int id)
        {
            var customer = unitOfWork.CustomerRepository.GetByID(id);

            if (customer==null)
            {
                throw new CustomerNotFoundException();
            }

            return customer;
        }

        public IEnumerable<Customer> GetCustomerAutoCompleteList(string searchKey)
        {
            var model = (unitOfWork.CustomerRepository.Get()
                  .Where(r => r.CustomerName.StartsWith(searchKey, StringComparison.InvariantCultureIgnoreCase))
                     .Select(r => new Customer
                     {
                         CustomerId = r.CustomerId,
                         CustomerName = r.CustomerName
                     }));

            return model;
        }

        public virtual void UpdateCustomer(Customer model)
        {
            if (model == null) { throw new CustomerNotFoundException(); }

            var editmodel = GetCustomerByID(model.CustomerId);

            if (editmodel == null) { throw new CustomerNotFoundException(); }

            editmodel.CustomerName = model.CustomerName;

            editmodel.CustomerEMail = model.CustomerEMail;

            editmodel.CustomerAddress = model.CustomerAddress;

            try
            {
                unitOfWork.CustomerRepository.Update(editmodel);

                unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    throw new MyPosDbException("Operation Could Not Carry Out Due To An Database Error", ex);
                }
                   
                throw ex;
            }
        }

    }
}
