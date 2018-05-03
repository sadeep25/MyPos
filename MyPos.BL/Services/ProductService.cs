using MyPos.BL.Exceptions;
using MyPos.DAL.Entity;
using MyPos.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

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

        public void UpdateProductQuantity(int productId, int productQuantity, bool isDeleted)
        {
            var editModel = GetProductByID(productId);
            if (editModel == null)
            {
                throw new ProductNotFoundException();
            }
            if (isDeleted)
            {
                editModel.ProductStockAvailable = editModel.ProductStockAvailable + productQuantity;
            }
            else
            {
                if (editModel.ProductStockAvailable >= productQuantity)
                {
                    editModel.ProductStockAvailable = editModel.ProductStockAvailable - productQuantity;                  
                }
                else
                {
                    { throw new ProductOutOfStockException("There is only " + editModel.ProductStockAvailable + " " + editModel.ProductName + " available in the stock"); }
                }               
            }
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
    }
}



