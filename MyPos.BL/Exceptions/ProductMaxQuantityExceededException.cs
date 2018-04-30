using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.BL.Exceptions
{
    public class ProductMaxQuantityExceededException:Exception
    {
        public ProductMaxQuantityExceededException():base("Order Quantity Can not exceed 10")
        {

        }
    }
}
