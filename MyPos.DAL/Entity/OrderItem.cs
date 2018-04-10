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
        public int ID { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public int ProductId { get; set; }


        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}
