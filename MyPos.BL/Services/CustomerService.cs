using MyPos.BL.Exceptions;
using MyPos.DAL.Entity;
using MyPos.DAL.Repository;
using System;
using System.Collections.Generic;
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
            if (model == null) { throw new MyPosException("Model can not be null !"); }
            unitOfWork.CustomerRepository.Insert(model);
            unitOfWork.Save();
        }

        public IEnumerable<Customer> GetCustomerList()
        {
            return unitOfWork.CustomerRepository.Get(null, null, null);
        }

        public Customer GetCustomerByID(int id)
        {
            return unitOfWork.CustomerRepository.Get(p => p.ID == id, null, null).FirstOrDefault();
        }

        public virtual void UpdateCustomer(Customer model)
        {
            if (model == null) { throw new MyPosException("Model can not be null !"); }
            var editmodel = GetCustomerByID(model.ID);
            if (editmodel == null) { throw new MyPosException("No matching Customer found!"); }

            editmodel.Name = model.Name;
            editmodel.EMail = model.EMail;
            editmodel.Address = model.Address;

            unitOfWork.CustomerRepository.Update(editmodel);
            unitOfWork.Save();
        }

    }
}
