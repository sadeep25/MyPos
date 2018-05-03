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
            var customerList = (unitOfWork.CustomerRepository.Get()
                  .Where(r => r.CustomerName.StartsWith(searchKey, StringComparison.InvariantCultureIgnoreCase))
                     .Select(r => new Customer
                     {
                         CustomerId = r.CustomerId,
                         CustomerName = r.CustomerName
                     }));
            if (customerList.Count()==0)
            {
                customerList = customerList.Concat(new Customer[] { new Customer() { CustomerId = -1, CustomerName = "There Is No Such A Customer In Database" } });
                return customerList;
            }
            else
            {
                return customerList;
            }         
        }
    }
}
