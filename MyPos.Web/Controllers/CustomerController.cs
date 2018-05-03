using MyPos.BL.Services;
using MyPos.DAL.Context;
using MyPos.DAL.Repository;

using System.Web.Mvc;

namespace MyPos.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomerController() 
            : this(new UnitOfWork(new MyPosDbContext()))
        { }

        public CustomerController(UnitOfWork unitOfWork)
        {
            this._customerService = new CustomerService(unitOfWork);
        }

        //Customer Auto Complete Drowdown Ajax
        [HttpPost]
        public ActionResult CustomerAutocomplete(string searchKey)
        {
            var model = _customerService.GetCustomerAutoCompleteList(searchKey);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}