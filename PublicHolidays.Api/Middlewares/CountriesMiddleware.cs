
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PublicHolidays.Services;
using Microsoft.Extensions.Options;

namespace PublicHolidays.Api.Middlewares
{
    public class CountriesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;
        private readonly CountriesMiddlewareOptions _options;

        public CountriesMiddleware(RequestDelegate next, ILoggerFactory logFactory,IOptions<CountriesMiddlewareOptions> options)
        {
            _next = next;
            _logger = logFactory.CreateLogger("MyMiddleware");
            _options = options.Value;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            await  _options.countryService.SaveCountriesIfNeeded();

            _logger.LogInformation("MyMiddleware executing..");

            await _next(httpContext); // calling next middleware

        }
    }

    public static class CountriesMiddlewareExtensions
    {
        public static IApplicationBuilder UseCountiresMiddleware(this IApplicationBuilder app,CountriesMiddlewareOptions options)
        {
            return app.UseMiddleware<CountriesMiddleware>(Options.Create(options));
        }
    }
}
