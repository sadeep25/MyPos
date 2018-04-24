using MyPos.DAL.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MyPos.Web.ViewModels
{
    public class OrderEditViewModel
    {
        public int OrderId { get; set; }

        public DateTime OrderDate { get; set; }

        [MaxLength(250)]
        public string OrderShippingAddress { get; set; }

        public int OrderTotal { get; set; }





        public virtual ICollection<OrderItem> OrderItems { get; set; }

        public virtual Customer OrderCustomer { get; set; }
    }
}