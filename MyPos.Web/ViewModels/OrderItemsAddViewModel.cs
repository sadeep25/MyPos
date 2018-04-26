using MyPos.DAL.Entity;
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

        public int OrderId { get; set; }

        public int OrderCustomerId { get; set; }

        public string OrderShippingAddress { get; set; }

        public DateTime OrderDate { get; set; }

        public int OrderTotal { get; set; }


        public virtual ICollection<OrderItem> OrderItems { get; set; }

    }
}