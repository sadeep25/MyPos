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
    public class OrderService
    {
        private UnitOfWork unitOfWork;

        public OrderService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Add(Order model)
        {
            if (model == null) { throw new MyPosException("Model can not be null !"); }
            unitOfWork.OrderRepository.Insert(model);
            unitOfWork.Save();
        }

        public Order GetOrderByID(int id)
        {
            return unitOfWork.OrderRepository.GetByID(id);
        }

        public virtual void UpdateOrder(Order model)
        {
            if (model == null) { throw new MyPosException("Model can not be null !"); }
            var editmodel = GetOrderByID(model.ID);
            if (editmodel == null) { throw new MyPosException("No matching Order found!"); }

            editmodel.OrderDate = model.OrderDate;
            editmodel.OrderItems = model.OrderItems;
            editmodel.ShippingAddress = model.ShippingAddress;
            editmodel.Customer = model.Customer;
            unitOfWork.OrderRepository.Update(editmodel);
            unitOfWork.Save();
        }


    }
}
