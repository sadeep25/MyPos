using MyPos.DAL.Entity;
using MyPos.DAL.Repository;
using MyPos.Web.ViewModels;
using System;
using System.Web.Mvc;
using AutoMapper;
using MyPos.Web.CustomAttributes;
using MyPos.Web.ErrorHandlers;
using MyPos.BL.Services;

using MyPos.DAL.Context;

namespace MyPos.Web.Controllers
{
    [MyPosErrorHandler]
    public class OrderController : Controller
    {
        //Declaring Service Variables
        private readonly OrderService _orderService;

        public OrderController()
           : this(new UnitOfWork(new MyPosDbContext()))
        { }

        //instantiating Services
        public OrderController(UnitOfWork unitOfWork)
        { 
            this._orderService = new OrderService(unitOfWork);
        }

        //Get: Add New Order 
        [HttpGet]
        [MyPosAuthorize]
        public ActionResult NewOrder()
        {
            OrderStartViewModel orderStartViewModel = new OrderStartViewModel();
            orderStartViewModel.RecentOrders = _orderService.GetRecentOrders();
            return View(orderStartViewModel);
        }

        //Get: Add New Order 
        [HttpPost]
        [MyPosAuthorize]
        public ActionResult NewOrder(OrderStartViewModel orderStartViewModel)
        {
            OrderViewModel order = new OrderViewModel();
            order.OrderCustomerId = orderStartViewModel.CustomerID;
            order.OrderDate = orderStartViewModel.OrderDate;
            if (ModelState.IsValid)
            {
                return RedirectToAction("ShoppingCart", order);
            }
            return View(orderStartViewModel);
        }

        //Get: Shopping Cart
        [HttpGet]
        [MyPosAuthorize]
        public ActionResult ShoppingCart(OrderViewModel orderViewModel)
        {
            return View(orderViewModel);
        }

        //Post: Shopping Cart
        [HttpPost]
        [ActionName("ShoppingCart")]
        [MyPosAuthorize]
        public ActionResult ShoppingCartSave(OrderViewModel orderViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderViewModel, Order>();
            });
            IMapper mapper = config.CreateMapper();
            var order = mapper.Map<OrderViewModel, Order>(orderViewModel);
            try
            {
                var orderId = _orderService.Add(order);
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("OrderDetails", "Order", new { id = orderId })
                });
               
            }catch(Exception ex)
            {
                return Json(new
                {
                    success = false,
                    errors = new {ex.Message}
                });
            }     
        }

        //Get: Order Details
        [HttpGet]
        [MyPosAuthorize]
        public ActionResult OrderDetails(int id=0)
        {
            var model = _orderService.GetOrderByID(id,true);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var order = mapper.Map<Order, OrderViewModel>(model);
            return View(order);
        }

        //Get: Order Edit
        [HttpGet]
        [MyPosAuthorize]
        public ActionResult OrderEdit(int id=0)
        {
            var model = _orderService.GetOrderByID(id,true);  
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var order = mapper.Map<Order, OrderViewModel>(model);
            return View(order);
        }

        //Post: Order Edit
        [HttpPost]
        [MyPosAuthorize]
        public ActionResult OrderEdit(OrderViewModel orderViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderViewModel, Order>();
            });
            IMapper mapper = config.CreateMapper();
            var order = mapper.Map<OrderViewModel, Order>(orderViewModel);
            try
            {
                _orderService.UpdateOrder(order);
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("OrderDetails", "Order", new { id = order.OrderId })
                });
            }catch(Exception ex)
            {
                return Json(new
                {
                    success = false,
                    errors = new { ex.Message }
                });
            }
        }

        //Post: Order Delete Ajax
        [HttpGet]
        [MyPosAuthorize]
        public ActionResult DeleteOrder(int orderId=0)
        {
            OrderStartViewModel orderStartViewModel = new OrderStartViewModel();
            orderStartViewModel.RecentOrders = _orderService.DeleteOrder(orderId);
            return PartialView("_RecentOrders", orderStartViewModel);
        }

    }
}