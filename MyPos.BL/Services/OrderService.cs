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
    public class OrderService
    {
        private UnitOfWork unitOfWork;

        public OrderService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Add(Order model)
        {
            if (model == null) { throw new OrderNotFoundException(); }

            try
            {
                unitOfWork.OrderRepository.Insert(model);
                unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    throw new MyPosDbException("Oops An Database Error Occured While Saving The Order", ex);
                }

                throw ex;
            }
        }

        public Order GetOrderByID(int id)
        {
            Order order = unitOfWork.OrderRepository.GetByID(id);

            if (order == null)
            {
                throw new OrderNotFoundException();
            }

            return order;
        }
        public void UpdateOrderTotal(int id, int itemSubTotal)
        {
            var editmodel = GetOrderByID(id);

            if (editmodel == null)
            {
                throw new OrderNotFoundException();
            }
            editmodel.OrderTotal = (editmodel.OrderTotal - itemSubTotal);

            try
            {
                unitOfWork.OrderRepository.Update(editmodel);

                unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    throw new MyPosDbException("Oops A Database Error Occured While Updateing The Order", ex);
                }

                throw ex;
            }

        }

        public int GetLatestOrderId()
        {
            int nextOrderId;

            int? currentOrderId = (unitOfWork.OrderRepository.Get()
               .OrderByDescending(x => x.OrderId)
              .FirstOrDefault()?.OrderId);

            if (currentOrderId == null)
            {
                nextOrderId = 1;
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
                .Where(x => x.OrderIsDeleted != true)
                .OrderByDescending(x => x.OrderId)
                .Select(r => new Order
                {
                    OrderCustomer = r.OrderCustomer,
                    OrderId = r.OrderId,
                    OrderDate = r.OrderDate,
                    OrderShippingAddress = r.OrderShippingAddress,
                    OrderTotal = r.OrderTotal
                })).Take(5);

            return orderList;
        }


        public int GetLatestOrderIDFromCustomerID(int id)
        {
            var order = unitOfWork.OrderRepository.Get()
                .Where(r => r.OrderCustomerId == id && r.OrderIsDeleted != true)
                .OrderByDescending(x => x.OrderId)
                .FirstOrDefault();

            if (order == null)
            {
                throw new OrderNotFoundException();
            }

            return order.OrderId;
        }


        public void DeleteOrder(int id)
        {
            var editmodel = GetOrderByID(id);

            if (editmodel == null)
            {
                throw new OrderNotFoundException();
            }

            editmodel.OrderIsDeleted = true;

            try
            {
                unitOfWork.OrderRepository.Update(editmodel);

                unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    throw new MyPosDbException("Oops A Database Error Occured While Deleting The Order", ex);
                }

                throw ex;
            }
        }

        public virtual void UpdateOrder(Order model)
        {
            if (model == null) { throw new OrderNotFoundException(); }

            var editmodel = GetOrderByID(model.OrderId);

            if (editmodel == null) { throw new OrderNotFoundException(); }

            var items = model.OrderItems.ToList();

            var i = 0;

            if (items.Count() > 0)
            {
                foreach (var orderItem in editmodel.OrderItems)
                {
                    if (orderItem.OrderItemIsDeleted == false)
                    {
                        orderItem.OrderItemProductId = items[i].OrderItemProductId;

                        orderItem.OrderItemQuantity = items[i].OrderItemQuantity;

                        orderItem.OrderItemTotalPrice = items[i].OrderItemTotalPrice;

                        i++;
                    }

                }
            }

            editmodel.OrderTotal = model.OrderTotal;

            try
            {
                unitOfWork.OrderRepository.Update(editmodel);

                unitOfWork.Save();
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    throw new MyPosDbException("Oops A Database Error Occured While Updating The Order", ex);

                }

                throw ex;
            }
        }


    }
}
