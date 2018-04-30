﻿using MyPos.BL.Exceptions;
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

        public OrderController()
           : this(new UnitOfWork())
        { }

        //instantiating Services
        public OrderController(UnitOfWork unitOfWork)
        { 
            this._orderService = new OrderService(unitOfWork);
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
            OrderViewModel order = new OrderViewModel();
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
        public ActionResult OrderItemsAdd(OrderViewModel orderViewModel)
        {
            return View(orderViewModel);
        }

        //Post: Oder Items Add
        [HttpPost]
        [ActionName("OrderItemsAdd")]
        public ActionResult OrderItemsAddPost(OrderViewModel orderViewModel)
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

        [HttpGet]
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

        [HttpGet]
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

        [HttpPost]
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



        [HttpGet]
        public ActionResult DeleteOrder(int orderId=0)
        {
            OrderStartViewModel orderStartViewModel = new OrderStartViewModel();
            orderStartViewModel.RecentOrders = _orderService.DeleteOrder(orderId);
            return PartialView("_RecentOrders", orderStartViewModel);
        }

    }
}