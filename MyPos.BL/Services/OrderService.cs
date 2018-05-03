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
                    if (item.OrderItemQuantity > 10)
                    {
                        throw new ProductMaxQuantityExceededException();
                    }
                    else
                    {
                        try
                        {
                            _productService.UpdateProductQuantity(item.OrderItemProductId, item.OrderItemQuantity, false);
                        }
                        catch (Exception exp)
                        {
                            transaction.Rollback();
                            throw exp;
                        }
                    }                   
                }
                if (model.OrderTotal > 5000)
                {
                    transaction.Rollback();
                    throw new OrderExceededMaxTotalException();
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

        public Order GetOrderByID(int id,bool withoutDeletedOrderItems)
        {
            Order order = unitOfWork.OrderRepository.GetByID(id);
            if (order == null)
            {
                throw new OrderNotFoundException();
            }
            if (withoutDeletedOrderItems)
            {
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
                  Order = r.Order
              }));
                order.OrderItems = orderItemList.ToList();               
                return order;
            }else
            {
                return order;
            }
        }

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

        public IEnumerable<Order> DeleteOrder(int id)
        {
            var editmodel = GetOrderByID(id,false);
            if (editmodel == null)
            {
                throw new OrderNotFoundException();
            }
            editmodel.OrderIsDeleted = true;
            try
            {
                unitOfWork.OrderRepository.Update(editmodel);
                unitOfWork.Save();
                return GetRecentOrders();
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
            var editmodel = GetOrderByID(model.OrderId,false);
            if (editmodel == null) { throw new OrderNotFoundException(); }
            var items = model.OrderItems.ToList();
            var i = 0;
            if (items.Count() > 0)
            {
                using (DbContextTransaction transaction = unitOfWork.DbContext.Database.BeginTransaction())
                {
                    foreach (var orderItem in editmodel.OrderItems)
                    {
                        if (orderItem.OrderItemIsDeleted == false)
                        {
                            if (orderItem.OrderItemProductId != items[i].OrderItemProductId)
                            {
                                try
                                {
                                    _productService.UpdateProductQuantity(orderItem.OrderItemProductId, orderItem.OrderItemQuantity, true);
                                    _productService.UpdateProductQuantity(items[i].OrderItemProductId, items[i].OrderItemQuantity, false);
                                }
                                catch (Exception exp)
                                {
                                    transaction.Rollback();
                                    throw exp;
                                }
                                orderItem.OrderItemProductId = items[i].OrderItemProductId;
                            }
                            //order Total limit exception
                            if (items[i].OrderItemQuantity > 10)
                            {
                                transaction.Rollback();
                                throw new ProductMaxQuantityExceededException();
                            }
                            else
                            {
                                orderItem.OrderItemQuantity = items[i].OrderItemQuantity;
                            }
                            orderItem.OrderItemTotalPrice = items[i].OrderItemTotalPrice;
                            if (items[i].OrderItemIsDeleted)
                            {
                                orderItem.OrderItemIsDeleted = items[i].OrderItemIsDeleted;
                                try
                                {
                                    _productService.UpdateProductQuantity(orderItem.OrderItemProductId, orderItem.OrderItemQuantity, true);
                                }
                                catch (Exception ex)
                                {
                                    transaction.Rollback();
                                    throw ex;
                                }
                            }
                            i++;
                        }
                    }
                    if (model.OrderTotal > 5000)
                    {
                        transaction.Rollback();
                        throw new OrderExceededMaxTotalException();
                    }
                    else
                    {
                        editmodel.OrderTotal = model.OrderTotal;
                    }
                    try
                    {
                        unitOfWork.OrderRepository.Update(editmodel);
                        unitOfWork.Save();
                        transaction.Commit();
                    }
                    catch (DbUpdateException ex)
                    {
                        transaction.Rollback();
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
    }
}
