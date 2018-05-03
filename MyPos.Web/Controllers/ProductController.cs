using MyPos.BL.Services;
using MyPos.DAL.Context;
using MyPos.DAL.Repository;

using System.Web.Mvc;

namespace MyPos.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController()
            :this(new UnitOfWork(new MyPosDbContext()))
        { }

        public ProductController(UnitOfWork unitOfWork)
        {
            this._productService = new ProductService(unitOfWork);
        }

        //Product Auto Complete Drowdown Ajax
        [HttpPost]
        public ActionResult ProductAutocompleteList(string searchKey)
        {
            var model = _productService.GetProductAutoCompleteList(searchKey);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        //Get Product Details Ajax
        [HttpPost]
        public ActionResult GetProductByID(int id)
        {
            var model = _productService.GetProductByID(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}