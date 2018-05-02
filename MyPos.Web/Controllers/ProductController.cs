using MyPos.BL.Services;
using MyPos.DAL.Repository;
using System.Web.Mvc;

namespace MyPos.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController()
            :this(new UnitOfWork())
        { }

        public ProductController(UnitOfWork unitOfWork)
        {
            this._productService = new ProductService(unitOfWork);
        }

       
        [HttpPost]
        public ActionResult ProductAutocompleteList(string searchKey)
        {
            var model = _productService.GetProductAutoCompleteList(searchKey);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public ActionResult GetProductByID(int id)
        {
            var model = _productService.GetProductByID(id);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}