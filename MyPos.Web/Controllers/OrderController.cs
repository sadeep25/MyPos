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
using AutoMapper;
using MyPos.Web.ErrorHandlers;

namespace MyPos.Web.Controllers
{
    [MyPosErrorHandler]
    public class OrderController : Controller
    {
        //Declaring Service Variables


        private readonly OrderService _orderService;

        private readonly CustomerService _customerService;


        private readonly OrderItemService _orderItemService;

        public OrderController()
           : this(new UnitOfWork())
        { }


        //instantiating Services
        public OrderController(UnitOfWork unitOfWork)
        {
            this._customerService = new CustomerService(unitOfWork);

            this._orderService = new OrderService(unitOfWork);



            this._orderItemService = new OrderItemService(unitOfWork);

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

            OrderItemsAddViewModel order = new OrderItemsAddViewModel();

            order.OrderCustomerId = orderStartViewModel.CustomerID;

            order.OrderDate = orderStartViewModel.OrderDate;

            if (ModelState.IsValid)
            {
                return RedirectToAction("OrderItemsAdd", order);
            }

            return View(orderStartViewModel);

        }





        //Get: Oder Items Add
        [HttpGet]
        public ActionResult OrderItemsAdd(OrderItemsAddViewModel orderItemsAddViewModel)
        {
            return View(orderItemsAddViewModel);
        }



        //Post: Oder Items Add
        [HttpPost]
        [ActionName("OrderItemsAdd")]
        public ActionResult OrderItemsAddPost(OrderItemsAddViewModel orderItemsAddViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderItemsAddViewModel, Order>();
            });
            IMapper mapper = config.CreateMapper();
            var dest = mapper.Map<OrderItemsAddViewModel, Order>(orderItemsAddViewModel);
            try
            {
                var orderId = _orderService.Add(dest);
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("OrderDetails", "Order", new { id = orderId })
                });
               
            }catch(ProductOutOfStockException ex)
            {
                return Json(new
                {
                    success = false,
                    errors = new {ex.Message}
                });
            }
          
          
        }

    


        [HttpGet]
        public ActionResult OrderDetails(int id=0)
        {
            var model = _orderService.GetOrderByID1(id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDetailsViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var order = mapper.Map<Order, OrderDetailsViewModel>(model);
            return View(order);
        }

        [HttpGet]
        public ActionResult OrderEdit(int id=0)
        {
            var model = _orderService.GetOrderByID1(id);
           
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderEditViewModel>();
            });
            IMapper mapper = config.CreateMapper();
            var order = mapper.Map<Order, OrderEditViewModel>(model);
            return View(order);
        }

        [HttpPost]
        public ActionResult OrderEdit(OrderEditViewModel orderEditViewModel)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderEditViewModel, Order>();
            });
            IMapper mapper = config.CreateMapper();
            var dest = mapper.Map<OrderEditViewModel, Order>(orderEditViewModel);


            if (ModelState.IsValid)
            {
                _orderService.UpdateOrder(dest); 
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("OrderDetails", "Order", new { id = orderEditViewModel.OrderId })
                });
            }
            else
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Keys.SelectMany(k => ModelState[k].Errors)
                                .Select(m => m.ErrorMessage).ToArray()
                });
            }
        }



        [HttpGet]
        public ActionResult DeleteOrder(int orderId=0)
        {
            //_orderservice should return list of recent order items just after delete order
            _orderService.DeleteOrder(orderId);

            OrderStartViewModel orderStartViewModel = new OrderStartViewModel();
            //this should go into delete order
            orderStartViewModel.RecentOrders = _orderService.GetRecentOrders();

            return PartialView("_RecentOrders", orderStartViewModel);

        }

    }
}