using MyPos.DAL.Entity;
using System;
using System.Collections.Generic;

namespace MyPos.Web.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderShippingAddress { get; set; }
        public int OrderTotal { get; set; }
        public int OrderCustomerId { get; set; }

        public virtual Customer OrderCustomer { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; }
       
    }
}