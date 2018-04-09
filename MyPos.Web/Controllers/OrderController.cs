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
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;
        private readonly ProductService _productService;

        public OrderController()
           : this(new UnitOfWork())
        { }

        public OrderController(UnitOfWork unitOfWork)
        {
            this._customerService = new CustomerService(unitOfWork);
            this._orderService = new OrderService(unitOfWork);
            this._productService = new ProductService(unitOfWork);

        }




        

        //Add New Order
        [HttpGet]
        public ActionResult AddNewOrder()
        {
            return View();
        }

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

        

        //Oder Items Add

        [HttpGet]
        public ActionResult OrderItemsAdd(OrderStartViewModel orderStartViewModel)
        {
            var orderItemsAddViewModel = new OrderItemsAddViewModel
            {
                CustomerID = orderStartViewModel.CustomerID,
                OrderDate = orderStartViewModel.OrderDate,
                OrderItems = new List<SingleItemViewModel>
                {
                    new SingleItemViewModel
                    {
                        ProductID=1,
                        ProductQuantity=30,
                        SubTotal=30
                    }

                }

            };


            return View(orderItemsAddViewModel);
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