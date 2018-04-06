using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPos.Web.ViewModels
{
    public class OrderStartViewModel
    {
        public int CustomerID { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
    }
}