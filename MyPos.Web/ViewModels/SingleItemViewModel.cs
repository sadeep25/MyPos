using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyPos.Web.ViewModels
{
    public class SingleItemViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductQuantity { get; set; }
        public int ProductPrice { get; set; }
        public int SubTotal { get; set; }
    }
}