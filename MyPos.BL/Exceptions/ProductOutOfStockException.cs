using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.BL.Exceptions
{
    public class ProductOutOfStockException:Exception
    {
        public ProductOutOfStockException()
           : base()
        { }

        public ProductOutOfStockException(string message)
            : base(message)
        { }

        public ProductOutOfStockException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
