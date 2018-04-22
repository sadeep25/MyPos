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
    public class ProductService
    {
        private UnitOfWork unitOfWork;

        public ProductService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }


        public Product GetProductByID(int id)
        {
            return unitOfWork.ProductRepository.GetByID(id);
        }

        public IEnumerable<Product> GetProductAutoCompleteList(string searchKey)
        {
            var productList = (unitOfWork.ProductRepository.Get()
                  .Where(r => r.ProductName.StartsWith(searchKey, StringComparison.InvariantCultureIgnoreCase))
                     .Select(r => new Product
                     {
                         ProductId = r.ProductId,
                         ProductName = r.ProductName
                     }));
            return productList;
        }


        public virtual void UpdatProductQuantityr(Product model, int QuantitySold)
        {
            if (model == null) { throw new MyPosException("Model can not be null !"); }
            var editmodel = GetProductByID(model.ProductId);
            if (editmodel == null) { throw new MyPosException("No matching Order found!"); }
            if (editmodel.ProductStockAvailable>=QuantitySold)
            {
                editmodel.ProductStockAvailable = editmodel.ProductStockAvailable - QuantitySold;
            }
            else
            {
                { throw new MyPosException("There not enough stock available to make this "); }
            }
            
            unitOfWork.Save();
        }

    }
}
