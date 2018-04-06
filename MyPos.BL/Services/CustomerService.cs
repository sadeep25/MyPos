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
        //private UnitOfWork unitOfWork;

        //public CustomerService(UnitOfWork unitOfWork)
        //{
        //    this.unitOfWork = unitOfWork;
        //}

        //public void Add(Customer model)
        //{
        //    if (model == null) { throw new MyPosException("Model can not be null !"); }
        //    if (GetProjectByName(model.ID, model.Name) != null) { throw new MyPosException("A project with the same name, already exists !"); }


        //    unitOfWork.CustomerRepository.Insert(model);
        //    unitOfWork.Save();
        //}

        //public IEnumerable<Customer> GetCustomerList()
        //{
        //    return unitOfWork.CustomerRepository.Get(null, null, null);
        //}

        //public IEnumerable<Customer> GetCustomerList(int customerId)
        //{
        //    return GetCustomerList().Where(m => (m.ID == customerId));
        //}

        //public Customer GetProjectByName(int customerId, string customerName, StringComparison compareCulture = StringComparison.CurrentCultureIgnoreCase)
        //{
        //    if (string.IsNullOrWhiteSpace(customerName)) { return null; }
        //    customerName = customerName.Trim();
        //    return GetCustomerList(customerId).FirstOrDefault(m => (m.Name.Equals(customerName, compareCulture)));
        //}



        //public Customer GetCustomerByID(int id)
        //{
        //    return unitOfWork.CustomerRepository.Get(p => p.ID == id, null, null).FirstOrDefault();
        //}

        //public Customer GetCustomerByID(int clientId, int id)
        //{
        //    return GetCustomerList(clientId).FirstOrDefault(m => (m.ID == id));
        //}





        //public virtual void UpdateCustomer(Customer model)
        //{
        //    ////if (model == null) { throw new MyPosException("Model can not be null !"); }
        //    //var editmodel = GetCustomerByID(model.ClientId, model.Id);
        //    ////if (editmodel == null) { throw new MyPosException("No matching project  found!"); }
        //    ////var matchignmodel = GetProjectByName(model.ClientId, model.Name);
        //    ////if (matchignmodel != null && matchignmodel.Id != model.Id) { throw new PencoException("A project with the same name, already exists !"); }


        //    //editmodel.Name = model.Name;
        //    //editmodel.EMail = model.EMail;
        //    //editmodel.Address = model.Address;
           

        //    //unitOfWork.CustomerRepository.Update(editmodel);
        //    //unitOfWork.Save();
        //}
    }
}
