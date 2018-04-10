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
    public class OrderController : Controller
    {
        //Declaring Service Variables
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;
        private readonly ProductService _productService;

        public OrderController()
           : this(new UnitOfWork())
        { }
        //instantiating Services
        public OrderController(UnitOfWork unitOfWork)
        {
            this._customerService = new CustomerService(unitOfWork);
            this._orderService = new OrderService(unitOfWork);
            this._productService = new ProductService(unitOfWork);

        }


        //Get: Add New Order 
        [HttpGet]
        public ActionResult AddNewOrder()
        {
            return View();
        }


        //Get: Add New Order 
        [HttpPost]
        public ActionResult AddNewOrder(OrderStartViewModel orderStartViewModel)
        {

            if (ModelState.IsValid)
            {
                return RedirectToAction("OrderItemsAdd", orderStartViewModel);
            }
            return View(orderStartViewModel);

        }
        //Add New Order AJAX Requests
        [HttpPost]
        public ActionResult CustomerAutocomplete(string searchKey)
        {
            var model = _customerService.GetCustomerAutoCompleteList(searchKey);
            return Json(model, JsonRequestBehavior.AllowGet);
        }




        //Get: Oder Items Add

        [HttpGet]
        public ActionResult OrderItemsAdd(OrderStartViewModel orderStartViewModel)
        {
            return View(orderStartViewModel);
        }



        //Post: Oder Items Add
        [HttpPost]
        public ActionResult OrderItemsAdd(Order order)
        {
            order.ShippingAddress = _customerService.GetCustomerByID(order.CustomerId).Address;
            if (ModelState.IsValid)
            {
                _orderService.Add(order);
                return RedirectToAction("AddNewOrder");
            }

            return View(order);
        }


        //Oder Item Add AJAX requests
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