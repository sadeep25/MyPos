using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPos.Web.ViewModels
{
    public class OrderItemsAddViewModel
    {
        public int CustomerID { get; set; }


        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public virtual List<SingleItemViewModel> OrderItems { get; set; }

    }
}