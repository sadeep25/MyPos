using MyPos.BL.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPos.Web.ErrorHandlers
{
    public class Test : HandleErrorAttribute
    {
        public override void OnException(ExceptionContext filterContext)
        {
            Exception ex = filterContext.Exception;
            if (ex is OrderNotFoundException)
            {
                filterContext.ExceptionHandled = true;
                var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error1",
                    ViewData = new ViewDataDictionary(model)
                };

            }
            else
            {
                filterContext.ExceptionHandled = true;
                var model = new HandleErrorInfo(filterContext.Exception, "Controller", "Action");

                filterContext.Result = new ViewResult()
                {
                    ViewName = "Error",
                    ViewData = new ViewDataDictionary(model)
                };
            }

        }

    }
}