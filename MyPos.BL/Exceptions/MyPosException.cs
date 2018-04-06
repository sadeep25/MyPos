using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyPos.BL.Exceptions
{
    public class MyPosException:Exception
    {
        public MyPosException()
            : base()
        { }

        public MyPosException(string message)
            : base(message)
        { }


        public MyPosException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
