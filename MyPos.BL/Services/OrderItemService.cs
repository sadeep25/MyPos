using MyPos.DAL.Entity;
using MyPos.DAL.Repository;
using System;
using System.Collections.Generic;
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
            return unitOfWork.OrderItemRepository.GetByID(id);
        }

        public IEnumerable<OrderItem> DeleteOrderItem(int id)
        {

            var editmodel = GetOrderItemByID(id);
            editmodel.OrderItemIsDeleted = true;
            unitOfWork.OrderItemRepository.Update(editmodel);
            unitOfWork.Save();
            return GetListOfOrderItemsByOrderId(editmodel.OrderItemOrderId);
        }

    }
}
