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
        public void UpdateOrderTotal(int id,int itemSubTotal)
        {
            var editmodel = GetOrderByID(id);
            editmodel.OrderTotal =(editmodel.OrderTotal-itemSubTotal);
            unitOfWork.OrderRepository.Update(editmodel);
            unitOfWork.Save();
        }

        public int GetLatestOrderId()
        {
            int nextOrderId;
            int? currentOrderId  = (unitOfWork.OrderRepository.Get()
               .OrderByDescending(x => x.OrderId)
              .FirstOrDefault()?.OrderId);
            if (currentOrderId == null)
            {
                nextOrderId = 0;
            }
            else
            {
                nextOrderId = (int)currentOrderId + 1;
            }
            return nextOrderId;
        }
        //new
        public IEnumerable<Order> GetRecentOrders()
        {
            var orderList = (unitOfWork.OrderRepository.Get()
                .Where(x=>x.OrderIsDeleted!=true)
                .OrderByDescending(x => x.OrderId)
                .Select(r => new Order
                {
                    OrderCustomer = r.OrderCustomer,
                    OrderId = r.OrderId,
                    OrderDate = r.OrderDate,
                    OrderShippingAddress = r.OrderShippingAddress,
                    OrderTotal=r.OrderTotal
                })).Take(5);


            return orderList;
        }
        //new
        public int GetLatestOrderIDFromCustomerID(int id)
        {
            var orderId = unitOfWork.OrderRepository.Get()
                .Where(r => r.OrderCustomerId == id && r.OrderIsDeleted!=true).OrderByDescending(x => x.OrderId).FirstOrDefault().OrderId;
            return orderId;
        }

        public void DeleteOrder(int id)
        {
            //unitOfWork.OrderRepository.Delete(id);
            //unitOfWork.Save();
            var editmodel = GetOrderByID(id);
            editmodel.OrderIsDeleted = true;
            unitOfWork.OrderRepository.Update(editmodel);
            unitOfWork.Save();
            //need to add an exception.
        }

        public virtual void UpdateOrder(Order model)
        {
            if (model == null) { throw new MyPosException("Model can not be null !"); }
            var editmodel = GetOrderByID(model.OrderId);
            if (editmodel == null) { throw new MyPosException("No matching Order found!"); }

            //editmodel.OrderDate = model.OrderDate;
            //editmodel.OrderItems = model.OrderItems;
            var items = model.OrderItems.ToList();
            
            var i=0;
            foreach (var orderItem in editmodel.OrderItems)
            {
                
                orderItem.OrderItemProductId = items[i].OrderItemProductId;
                orderItem.OrderItemQuantity = items[i].OrderItemQuantity;
                orderItem.OrderItemTotalPrice = items[i].OrderItemTotalPrice;
                i++;
            }
            editmodel.OrderTotal = model.OrderTotal;
            //editmodel.OrderShippingAddress = model.OrderShippingAddress;
            //editmodel.OrderCustomerId = model.OrderCustomerId;
            unitOfWork.OrderRepository.Update(editmodel);
            unitOfWork.Save();
        }


    }
}
