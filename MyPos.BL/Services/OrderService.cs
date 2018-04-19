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
        //new
        public IEnumerable<Order> GetRecentOrders()
        {
            var orderList = (unitOfWork.OrderRepository.Get()
                .OrderByDescending(x => x.ID)
                .Select(r => new Order
                {
                    Customer = r.Customer,
                    ID = r.ID,
                    OrderDate = r.OrderDate,
                    ShippingAddress = r.ShippingAddress
                })).Take(5);


            return orderList;
        }
        //new
        public int GetLatestOrderIDFromCustomerID(int id)
        {
            var orderId = unitOfWork.OrderRepository.Get()
                .Where(r => r.CustomerId == id).OrderByDescending(x => x.ID).FirstOrDefault().ID;
            return orderId;
        }

        public virtual void UpdateOrder(Order model)
        {
            if (model == null) { throw new MyPosException("Model can not be null !"); }
            var editmodel = GetOrderByID(model.ID);
            if (editmodel == null) { throw new MyPosException("No matching Order found!"); }

            editmodel.OrderDate = model.OrderDate;
            editmodel.OrderItems = model.OrderItems;
            editmodel.ShippingAddress = model.ShippingAddress;
            editmodel.CustomerId = model.CustomerId;
            unitOfWork.OrderRepository.Update(editmodel);
            unitOfWork.Save();
        }


    }
}
