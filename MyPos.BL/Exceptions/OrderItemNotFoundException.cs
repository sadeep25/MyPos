using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.BL.Exceptions
{
    public class OrderItemNotFoundException : Exception
    {
        public OrderItemNotFoundException() : base("Oops OrderItem Not Found")
        {

        }
    }
}
