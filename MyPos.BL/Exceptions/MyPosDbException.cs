using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.BL.Exceptions
{
    public class MyPosDbException : Exception
    {
        
        public MyPosDbException(string message) : base(message)
        {

        }
        public MyPosDbException(string message, Exception innerException) : base(message, innerException)
        {

        }

    }
}
