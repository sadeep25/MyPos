using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.BL.Exceptions
{
    public class OrderExceededMaxTotalException : Exception
    {
        public OrderExceededMaxTotalException():base("Order Exceeded Max Total of 5000 Rs Per Order")
        {
                
        }
    
    }
   
}
