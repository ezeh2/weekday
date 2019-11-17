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
            // allow content from:
            //  'self'
            //  stackpath.bootstrapcdn.com
            //  ajax.aspnetcdn.com
            // 
            // 'unsafe-inline' is needed for <link  asp-fallback-href="..." asp-fallback-test-class="..."  />
            context.HttpContext.Response.Headers.Add("Content-Security-Policy"
                , new Microsoft.Extensions.Primitives.StringValues("default-src 'self' stackpath.bootstrapcdn.com ajax.aspnetcdn.com 'unsafe-eval' 'unsafe-inline';"));

            // Strict-Transport-Security: already configured with IApplicationBuilder.UseHsts();
        }
    }
}
