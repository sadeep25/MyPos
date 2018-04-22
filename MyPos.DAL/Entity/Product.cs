using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.DAL.Entity
{
    public class Product
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }

        [Required]
        [MaxLength(60)]
        public string ProductName { get; set; }

        [Required]
        [MaxLength(60)]
        public string ProductDescription { get; set; }

        [Required]
        public Decimal ProductCurrentPrice { get; set; }

        [Required]
        public int ProductStockAvailable { get; set; }




    }
}
