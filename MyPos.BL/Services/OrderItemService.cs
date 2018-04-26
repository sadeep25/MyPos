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
    public class OrderItemService
    {
        private UnitOfWork unitOfWork;

        public OrderItemService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IEnumerable<OrderItem> GetListOfOrderItemsByOrderId(int id)
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

                    Product = r.Product

                }));

            return orderItemList;

        }
        public OrderItem GetOrderItemByID(int id)
        {
            var orderItem = unitOfWork.OrderItemRepository.GetByID(id);

            if (orderItem == null)
            {
                throw new OrderItemNotFoundException();
            }

            return orderItem;
        }

        public IEnumerable<OrderItem> DeleteOrderItem(int id)
        {
            var editmodel = GetOrderItemByID(id);

            if (editmodel == null)
            {
                throw new OrderItemNotFoundException();
            }

            editmodel.OrderItemIsDeleted = true;

            try
            {
                unitOfWork.OrderItemRepository.Update(editmodel);

                unitOfWork.Save();
                
            }
            catch (DbUpdateException ex)
            {
                var sqlException = ex.GetBaseException() as SqlException;

                if (sqlException != null && sqlException.Number == 547)
                {
                    throw new MyPosDbException("Deleting Order Item Could Not Carry Out Due To An Database Error", ex);
                }

                throw ex;
            }

            return GetListOfOrderItemsByOrderId(editmodel.OrderItemOrderId);
        }

    }
}
