using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyPos.DAL.Entity;

namespace MyPos.DAL.Entity
{
   
    public class Customer
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }

        [Required]
        [MaxLength(150)]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [EmailAddress]
        [MaxLength(250)]
        public string CustomerEMail { get; set; }

        [MaxLength(250)]
        public string CustomerAddress { get; set; }

       
    }
}
