using MyPos.BL.Exceptions;
using MyPos.DAL.Entity;
using MyPos.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;

namespace MyPos.BL.Services
{
    public class OrderService
    {
        private UnitOfWork unitOfWork;
        private readonly ProductService _productService;
        private readonly CustomerService _customertService;
        public OrderService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            this._productService = new ProductService(unitOfWork);
           this._customertService = new CustomerService(unitOfWork);
        }

        public int Add(Order model)
        {
            if (model == null) { throw new OrderNotFoundException(); }
            model.OrderShippingAddress = _customertService.GetCustomerByID(model.OrderCustomerId).CustomerAddress;
            using (DbContextTransaction transaction = unitOfWork.DbContext.Database.BeginTransaction())
            {
                foreach (var item in model.OrderItems)
                {
                    try
                    {
                        _productService.UpdatProductQuantity(item.OrderItemProductId, item.OrderItemQuantity);
                    }
                    catch (Exception exp)
                    {
                        throw exp;
                    }
                }
                try
                {
                    unitOfWork.OrderRepository.Insert(model);
                    unitOfWork.Save();
                    transaction.Commit();
                }
                catch (DbUpdateException ex)
                {
                    transaction.Rollback();
                    var sqlException = ex.GetBaseException() as SqlException;
                    if (sqlException != null && sqlException.Number == 547)
                    {
                        throw new MyPosDbException("Oops An Database Error Occured While Saving The Order", ex);
                    }
                    throw ex;
                }
                return GetRecentOrders().FirstOrDefault().OrderId;
            }
        }

        public Order GetOrderByID(int id)
        {
            Order order = unitOfWork.OrderRepository.GetByID(id);

           
            return order;
        } public Order GetOrderByID1(int id)
        {
            Order order = unitOfWork.OrderRepository.GetByID(id);

            var orderItemList = (unitOfWork.OrderItemRepository.Get()
                .Where(x => x.OrderItemIsDeleted != true && x.OrderItemOrderId == id)
                .Select(r => new OrderItem
                {

                    OrderItemId = r.OrderItemId,

                    OrderItemProductId = r.OrderItemProductId,

                    OrderItemQuantity = r.OrderItemQuantity,

                    OrderItemTotalPrice = r.OrderItemTotalPrice,

                    OrderItemOrderId = r.OrderItemOrderId,

                    Product = r.Product,
                    OrderItemIsDeleted = r.OrderItemIsDeleted,
                    Order=r.Order

                }));
            order.OrderItems = orderItemList.ToList();
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


        //public int GetLatestOrderId()
        //{
        //    int nextOrderId;
        //    int? currentOrderId = (unitOfWork.OrderRepository.Get()
        //       .OrderByDescending(x => x.OrderId)
        //      .FirstOrDefault()?.OrderId);
        //    if (currentOrderId == null)
        //    {
        //        nextOrderId = 1;
        //    }
        //    else
        //    {
        //        nextOrderId = (int)currentOrderId + 1;
        //    }
        //    return nextOrderId;
        //}


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

        public void UpdateOrder(Order model)
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
                        orderItem.OrderItemIsDeleted = items[i].OrderItemIsDeleted;
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
