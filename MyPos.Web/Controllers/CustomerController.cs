using MyPos.BL.Services;
using MyPos.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPos.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        // GET: Customer
        public CustomerController() : this(new UnitOfWork())
        {

        }

        public CustomerController(UnitOfWork unitOfWork)
        {
            this._customerService = new CustomerService(unitOfWork);
        }

        //Add New Order AJAX Requests
        [HttpPost]
        public ActionResult CustomerAutocomplete(string searchKey)
        {
            var model = _customerService.GetCustomerAutoCompleteList(searchKey);

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}