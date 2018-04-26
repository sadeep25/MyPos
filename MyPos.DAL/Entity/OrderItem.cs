using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.DAL.Entity
{
    public class OrderItem
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        public int OrderItemQuantity { get; set; }
        
        public int OrderItemTotalPrice { get; set; }

        public bool OrderItemIsDeleted { get; set; }

        public int OrderItemProductId { get; set; }

        public int OrderItemOrderId { get; set; }



        [ForeignKey("OrderItemProductId")]
        public virtual Product Product { get; set; }

        [ForeignKey("OrderItemOrderId")]
        public virtual Order Order { get; set; }
    }
}
