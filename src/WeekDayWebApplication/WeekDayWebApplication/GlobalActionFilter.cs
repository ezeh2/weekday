using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeekDayWebApplication
{
    public class GlobalActionFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection
            context.HttpContext.Response.Headers.Add("X-XSS-Protection"
                , new Microsoft.Extensions.Primitives.StringValues("1"));
            // https://developer.mozilla.org/de/docs/Web/HTTP/Headers/X-Frame-Options
            context.HttpContext.Response.Headers.Add("X-Frame-Options"
                , new Microsoft.Extensions.Primitives.StringValues("deny"));
            // https://developer.mozilla.org/de/docs/Web/HTTP/Headers/X-Content-Type-Options
            context.HttpContext.Response.Headers.Add("X-Content-Type-Options"
                , new Microsoft.Extensions.Primitives.StringValues("nosniff"));
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP
            /*
            Refused to load the stylesheet 'https://stackpath.bootstrapcdn.com/bootstrap/3.4.1/css/bootstrap.min.css' 
            because it violates the following Content Security Policy directive: "default-src 'self'". 
            Note that 'style-src-elem' was not explicitly set, so 'default-src' is used as a fallback.            */
            // context.HttpContext.Response.Headers.Add("Content-Security-Policy"
            //    , new Microsoft.Extensions.Primitives.StringValues("script-src ajax.aspnetcdn.com stackpath.bootstrapcdn.com;"));
        }
    }
}
