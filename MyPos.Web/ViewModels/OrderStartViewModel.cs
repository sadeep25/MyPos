using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MyPos.Web.ViewModels
{
    public class OrderStartViewModel
    {
        public int CustomerID { get; set; }
        [DisplayName("Customer Name : ")]
        [Required(ErrorMessage = "Customer Name Is A Required Field")]
        public string CustomerName { get; set; }
        [DisplayName("Order Date : ")]
        [Required(ErrorMessage = "Order Date Is A Required Field")]
        [DataType(DataType.DateTime,ErrorMessage ="Please Enter A Valid Date")]
        public DateTime OrderDate { get; set; }
    }
}