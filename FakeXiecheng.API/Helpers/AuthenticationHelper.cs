using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace FakeXiecheng.API.Helpers
{
    public static class AuthenticationHelper
    {
        public static Func<RedirectContext<CookieAuthenticationOptions>, Task> CookieAuthReplaceRedirector(
            HttpStatusCode statusCode,
            Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector
        )
        => context =>
        {
            if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = (int)statusCode;
                return Task.CompletedTask;
            }
            return existingRedirector(context);
        };
    }
}
