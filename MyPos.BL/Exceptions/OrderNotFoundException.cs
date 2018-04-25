using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.BL.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        public OrderNotFoundException():base("Requested Order Not Found")
        {

        }
    }
}
