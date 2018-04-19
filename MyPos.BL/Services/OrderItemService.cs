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
        
        public void DeleteOrderItem(int id)
        {
            unitOfWork.OrderItemRepository.Delete(id);
            unitOfWork.Save();
            //need to add an exception.
        }

    }
}
