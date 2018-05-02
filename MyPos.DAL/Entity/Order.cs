using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyPos.DAL.Entity
{
    [Serializable]
    public class Order
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        [MaxLength(250)]
        public string OrderShippingAddress { get; set; }
        public int OrderCustomerId { get; set; }
        public int OrderTotal { get; set; }
        public bool OrderIsDeleted { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; }
        [ForeignKey("OrderCustomerId")]
        public virtual Customer OrderCustomer { get; set; }
    }
}
