using MyPos.BL.Exceptions;
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
            OrderStartViewModel orderStartViewModel = new OrderStartViewModel();
            
            orderStartViewModel.RecentOrders = _orderService.GetRecentOrders();

            return View(orderStartViewModel);
        }


        //Get: Add New Order 
        [HttpPost]
        public ActionResult AddNewOrder(OrderStartViewModel orderStartViewModel)
        {
            Order order = new Order();
            order.CustomerId = orderStartViewModel.CustomerID;
            order.OrderDate = orderStartViewModel.OrderDate;

            if (ModelState.IsValid)
            {
                return RedirectToAction("OrderItemsAdd", order);
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

        public ActionResult OrderItemsAdd(Order order)
        {
            return View(order);
        }



        //Post: Oder Items Add
        [HttpPost]
        [ActionName("OrderItemsAdd")]
        public ActionResult OrderItemsAddPost(Order order)
        {
            order.ShippingAddress = _customerService.GetCustomerByID(order.CustomerId).Address;

            foreach (var item in order.OrderItems)
            {
                try
                {
                    _productService.UpdatProductQuantityr(_productService.GetProductByID(item.ProductId), item.Quantity);
                }
                catch (MyPosException exp)
                {
                    ModelState.AddModelError("error", exp.Message);
                }

            }

            if (ModelState.IsValid)
            {
                _orderService.Add(order);
                int NewOrderId = _orderService.GetLatestOrderIDFromCustomerID(order.CustomerId);
                //return RedirectToAction("AddNewOrder");
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("OrderDetails", "Order", new { id=NewOrderId})
                });
            }

            return Json(new

            {
                success = false,
                errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                .Select(m => m.ErrorMessage).ToArray()
            });
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
        [HttpGet]

        public ActionResult OrderDetails(int Id)
        {
            var model = _orderService.GetOrderByID(Id);
            return View(model);
        }

        [HttpGet]

        public ActionResult OrderEdit(int Id)
        {
            var model = _orderService.GetOrderByID(Id);
            return View(model);
        }

        [HttpPost]

        public ActionResult OrderEdit(Order order)
        {
            if (ModelState.IsValid)
            {
                _orderService.UpdateOrder(order);
                return RedirectToAction("OrderDetails", "Order", new { id = order.ID });
            }
            else
            {
                return View(order);
            }

           

        }

        //// GET: tblDepartMents/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    tblDepartMent tblDepartMent = db.tblDepartMents.Find(id);
        //    if (tblDepartMent == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(tblDepartMent);
        //}

        //// POST: tblDepartMents/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    tblDepartMent tblDepartMent = db.tblDepartMents.Find(id);
        //    db.tblDepartMents.Remove(tblDepartMent);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

    }
}