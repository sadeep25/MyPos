using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPos.DAL.Entity;
namespace MyPos.DAL.Entity
{
    public class Order
    {
        [Key]
        [Column(Order = 1)]
        public int ID { get; set; }

        public virtual  Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public string ShippingAddress { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
    }
}
