using MyPos.BL.Services;
using MyPos.DAL.Entity;
using MyPos.DAL.Repository;
using MyPos.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPos.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;
        private readonly ProductService _productService;

        public HomeController()
           : this(new UnitOfWork())
        { }


        public HomeController(UnitOfWork unitOfWork)
        {
            this._customerService = new CustomerService(unitOfWork);
            this._orderService = new OrderService(unitOfWork);
            this._productService = new ProductService(unitOfWork);

        }


        [HttpPost]
        public ActionResult CustomerAutocomplete(string searchKey)
        {
            var model = _customerService.GetCustomerAutoCompleteList(searchKey);


            return Json(model, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult CreateOrder()
        {
            return View();
        }


        [HttpPost]
        public ActionResult CreateOrder(OrderStartViewModel cu)
        {

            if (ModelState.IsValid)
            {

                return RedirectToAction("OrderAddItem", cu);
            }
            return View(cu);

        }
        //Oder Item Add

        [HttpPost]
        public ActionResult ProductAutocompleteList(string searchKey)
        {
            var model =_productService.GetProductAutoCompleteList(searchKey);


            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetProductByID(int id)
        {
            var model =_productService.GetProductByID(id);


            return Json(model, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult OrderAddItem(OrderStartViewModel orderStartViewModel)
        {
            var model = new OrderItemsAddViewModel
            {
                CustomerID = orderStartViewModel.CustomerID,
                OrderDate = orderStartViewModel.OrderDate,
                //OrderItems = new List<SingleItemViewModel>
                //{
                //    new SingleItemViewModel
                //    {
                //        ProductID=1,
                //        ProductQuantity=30,
                //        SubTotal=30
                //    }

                //}

            };


            return View(model);
        }

    }
}