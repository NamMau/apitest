using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.SqlServer.Server;

namespace webshop.MiddleWare
{
    public class SecretKeyMiddleware
    {
        private readonly RequestDelegate _next;

        public SecretKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue("secret_key", out var extractedSecretKey))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Secret key is missing");
                return;
            }

            if (!string.Equals(extractedSecretKey, "123456", StringComparison.OrdinalIgnoreCase))
            {
                context.Response.StatusCode = 403; // Forbidden
                await context.Response.WriteAsync("Unauthorized client");
                return;
            }

            await _next(context);
        }
    }
}
