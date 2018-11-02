using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Middleware
{
    public class ApiKeyValidatorMiddleware 
    {
        private const string ApiKeyToCheck = "7E8E799E-AD3A-4A31-B5F8-8BFC25F5A606";

        private RequestDelegate _next;

        public ApiKeyValidatorMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.Keys.Contains("api-key"))
            {
                context.Response.StatusCode = 400; //Bad Request                
                await context.Response.WriteAsync("Api Key is missing");
                return;
            }
            else
            {
                if (context.Request.Headers["api-key"] != ApiKeyToCheck)
                {
                    context.Response.StatusCode = 401; //UnAuthorized
                    await context.Response.WriteAsync("Invalid User Key");
                    return;
                }
            }

            await _next.Invoke(context);
        }
    }

    #region ExtensionMethod
    public static class ApiKeyValidatorsExtension
    {
        public static IApplicationBuilder ApplyApiKeyValidation(this IApplicationBuilder app)
        {
            app.UseMiddleware<ApiKeyValidatorMiddleware>();
            return app;
        }
    }
    #endregion
}
