using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.BL.Exceptions
{
    class ProductNotFoundException:Exception
    {
        public ProductNotFoundException() : base("Requested Product Not Found")
        {

        }
    }
}
