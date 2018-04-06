using MyPos.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPos.Web.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public ActionResult CreateOrder()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateOrder(string Prefix)
        {
            //Note : you can bind same list from database
            List<City> ObjList = new List<City>()
            {

                new City {Id=1,Name="Latur" },
                new City {Id=2,Name="Mumbai" },
                new City {Id=3,Name="Pune" },
                new City {Id=4,Name="Delhi" },
                new City {Id=5,Name="Dehradun" },
                new City {Id=6,Name="Noida" },
                new City {Id=7,Name="New Delhi" }

        };
            //Searching records from list using LINQ query
            var CityName = (from N in ObjList
                            where N.Name.StartsWith(Prefix)
                          select new { N.Name,N.Id });
            return Json(CityName, JsonRequestBehavior.AllowGet);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}