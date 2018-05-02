using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public int ProductCurrentPrice { get; set; }
        [Required]
        public int ProductStockAvailable { get; set; }
    }
}
