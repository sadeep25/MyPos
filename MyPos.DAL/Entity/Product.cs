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
        public int ID { get; set; }

        public string ProductName { get; set; }

        public string ProductDescription { get; set; }

        public Decimal CurrentPrice { get; set; }

        public int StockAvailable { get; set; }




    }
}
