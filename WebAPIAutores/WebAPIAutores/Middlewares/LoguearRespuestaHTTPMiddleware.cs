using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace WebAPIAutores.Middlewares
{
    public static class LoguearRespuestaHTTPMiddlewareExtensions
    {
        public static IApplicationBuilder UseLogearRespuestaHTTP(this IApplicationBuilder app)
        {
            return app.UseMiddleware<LoguearRespuestaHTTPMiddleware>();
        }
    }
    public class LoguearRespuestaHTTPMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<LoguearRespuestaHTTPMiddleware> _logger;
        
        public LoguearRespuestaHTTPMiddleware(RequestDelegate next, ILogger<LoguearRespuestaHTTPMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            using (var ms = new MemoryStream())
            {
                var originalBody = context.Response.Body;
                context.Response.Body = ms;
                await _next(context);

                ms.Seek(0, SeekOrigin.Begin);
                string answer = new StreamReader(ms).ReadToEnd();
                ms.Seek(0, SeekOrigin.Begin);

                await ms.CopyToAsync(originalBody);
                context.Response.Body = originalBody;
                _logger.LogInformation(answer);
            }
        }
    }
}