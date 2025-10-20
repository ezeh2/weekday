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
            context.HttpContext.Response.Headers["X-XSS-Protection"] = "1";
            // https://developer.mozilla.org/de/docs/Web/HTTP/Headers/X-Frame-Options
            context.HttpContext.Response.Headers["X-Frame-Options"] = "deny";
            // https://developer.mozilla.org/de/docs/Web/HTTP/Headers/X-Content-Type-Options
            context.HttpContext.Response.Headers["X-Content-Type-Options"] = "nosniff";

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP
            // allow content from:
            //  'self'
            //  stackpath.bootstrapcdn.com
            //  ajax.aspnetcdn.com
            // 
            // 'unsafe-inline' is needed for <link  asp-fallback-href="..." asp-fallback-test-class="..."  />
            context.HttpContext.Response.Headers["Content-Security-Policy"] = "default-src 'self' stackpath.bootstrapcdn.com ajax.aspnetcdn.com;";

            // Strict-Transport-Security: already configured with IApplicationBuilder.UseHsts();
        }
    }
}
