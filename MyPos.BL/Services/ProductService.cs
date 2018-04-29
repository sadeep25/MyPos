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
    public class ProductService
    {
        private UnitOfWork unitOfWork;

        public ProductService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public Product GetProductByID(int id)
        {
            var product = unitOfWork.ProductRepository.GetByID(id);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            return product;
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
            if (productList.Count() == 0)
            {
                productList = productList.Concat(new Product[] { new Product() { ProductId = -1, ProductName = "There Is No Such A Product In Database" } });
                return productList;
            }
            else
            {
                return productList;
            }
            
        }

        public void UpdateProductReturnedQuantity(int productId, int returnedQuantity)
        {
            var editModel = GetProductByID(productId);

            if (editModel == null)
            {
                throw new ProductNotFoundException();
            }

            editModel.ProductStockAvailable = editModel.ProductStockAvailable + returnedQuantity;

            try
            {
                unitOfWork.ProductRepository.Update(editModel);

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

        public virtual void UpdatProductQuantity(int productId, int quantitySold)
        {          
            var editmodel = GetProductByID(productId);
            if (editmodel == null) { throw new MyPosException("No matching Order found!"); }
            if (editmodel.ProductStockAvailable >= quantitySold)
            {
                editmodel.ProductStockAvailable = (editmodel.ProductStockAvailable - quantitySold); try
                {
                    unitOfWork.ProductRepository.Update(editmodel);
                    unitOfWork.Save();
                }
                catch (DbUpdateException ex)
                {
                    var sqlException = ex.GetBaseException() as SqlException;
                    if (sqlException != null && sqlException.Number == 547)
                    {
                        throw new MyPosDbException("Opps An Database Error While Making The Order", ex);
                    }
                   throw ex;
                }
            }
            else
            {
                { throw new ProductOutOfStockException("There not enough "+editmodel.ProductName+" stock available to make this order "); }
            }
           
        }

    }
}
