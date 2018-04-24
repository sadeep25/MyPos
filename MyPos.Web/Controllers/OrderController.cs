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
namespace MyPos.Web.Controllers
{
    public class OrderController : Controller
    {
        //Declaring Service Variables
        private readonly CustomerService _customerService;
        private readonly OrderService _orderService;
        private readonly ProductService _productService;
        private readonly OrderItemService _orderItemService;

        public OrderController()
           : this(new UnitOfWork())
        { }


        //instantiating Services
        public OrderController(UnitOfWork unitOfWork)
        {
            this._customerService = new CustomerService(unitOfWork);
            this._orderService = new OrderService(unitOfWork);
            this._productService = new ProductService(unitOfWork);
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
            //Order order = new Order();
            OrderItemsAddViewModel order = new OrderItemsAddViewModel();
            order.OrderCustomerId = orderStartViewModel.CustomerID;
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
        //must be changed
        public ActionResult OrderItemsAdd(OrderItemsAddViewModel orderItemsAddViewModel)
        {
            //var config = new MapperConfiguration(cfg =>
            //{
            //    cfg.CreateMap<OrderStartViewModel, OrderEditViewModel>();
            //});

            //IMapper mapper = config.CreateMapper();

            //var dest = mapper.Map<OrderStartViewModel, OrderEditViewModel>(orderStartViewModel);
            return View(orderItemsAddViewModel);
        }



        //Post: Oder Items Add
        [HttpPost]
        [ActionName("OrderItemsAdd")]
        public ActionResult OrderItemsAddPost(OrderItemsAddViewModel orderItemsAddViewModel)
        {
            Order order = new Order();

            orderItemsAddViewModel.OrderShippingAddress = _customerService.GetCustomerByID(orderItemsAddViewModel.OrderCustomerId).CustomerAddress;

            //this needs to change to a function that checks whether available stocks are enough
            var nextOrderId = _orderService.GetLatestOrderId();
            orderItemsAddViewModel.OrderId = nextOrderId;

            //start
            //var model = _orderService.GetOrderByID(orderStartViewModel.);
            //model.OrderItems = _orderItemService.GetListOfOrderItemsByOrderId(Id).ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<OrderItemsAddViewModel, Order>();
            });

            IMapper mapper = config.CreateMapper();

            var dest = mapper.Map<OrderItemsAddViewModel, Order>(orderItemsAddViewModel);
            //end
            foreach (var item in dest.OrderItems)
            {

                item.OrderItemOrderId = nextOrderId;
                try
                {
                    _productService.UpdatProductQuantityr(_productService.GetProductByID(item.OrderItemProductId), item.OrderItemQuantity);
                }
                catch (MyPosException exp)
                {
                    ModelState.AddModelError("error", exp.Message);
                }

            }

            if (ModelState.IsValid)
            {
                _orderService.Add(dest);

                //return RedirectToAction("AddNewOrder");
                return Json(new
                {
                    success = true,
                    redirectUrl = Url.Action("OrderDetails", "Order", new { id = nextOrderId })
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
            model.OrderItems = _orderItemService.GetListOfOrderItemsByOrderId(Id).ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderDetailsViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            var dest = mapper.Map<Order, OrderDetailsViewModel>(model);
            return View(dest);
        }



        [HttpGet]

        public ActionResult OrderEdit(int Id)
        {
            var model = _orderService.GetOrderByID(Id);
            model.OrderItems = _orderItemService.GetListOfOrderItemsByOrderId(Id).ToList();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderEditViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            var dest = mapper.Map<Order, OrderEditViewModel>(model);

            return View(dest);
        }



        [HttpPost]
        //must be changed to reflect All the changes
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
                //return RedirectToAction("OrderDetails", "Order", new { id = dest.OrderId });
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



     
        [HttpPost]
        public ActionResult DeleteOrderItem(int OrderItemId, int OrderId, int ItemSubTotal)
        {


            _orderService.UpdateOrderTotal(OrderId, ItemSubTotal);
            var model = _orderService.GetOrderByID(OrderId);
            var orderItemList = _orderItemService.DeleteOrderItem(OrderItemId);

            model.OrderItems = orderItemList.ToList();

            //
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Order, OrderEditViewModel>();
            });

            IMapper mapper = config.CreateMapper();

            var dest = mapper.Map<Order, OrderEditViewModel>(model);
            //

            return PartialView("_OrderItemList", dest);

        }



        [HttpGet]
        public ActionResult DeleteOrder(int OrderId)
        {
            //_orderItemService.DeleteOrderItem(OrderItemId);
            _orderService.DeleteOrder(OrderId);
            OrderStartViewModel orderStartViewModel = new OrderStartViewModel();

            orderStartViewModel.RecentOrders = _orderService.GetRecentOrders();

            return PartialView("_RecentOrders", orderStartViewModel);


        }

    }
}