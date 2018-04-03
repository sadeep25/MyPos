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
    [Serializable()]
    public class Customer
    {
        [Key]
        [Column(Order=1)]
        public int ID { get; set; }

        [Required]
        [MaxLength(150)]
        public string Name { get; set; }

        [EmailAddress]
        [MaxLength(250)]
        public string EMail { get; set; }

        [MaxLength(250)]
        public string Address { get; set; }

        public ICollection<Order> Orders { get; set; }
        
    }
}
