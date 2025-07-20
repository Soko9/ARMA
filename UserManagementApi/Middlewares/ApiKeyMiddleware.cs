namespace UserManagementApi.Middlewares
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        public const string ApiKeyHeaderName = "X-API-KEY";

        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext HttpContext, IConfiguration Configs)
        {
            if (!HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var ApiKey))
            {
                HttpContext.Response.StatusCode = 401; // Unauthorized
                await HttpContext.Response.WriteAsync("API Key is missing");
                return;
            }

            string? ValidApiKey = Configs["ApiKey:key"];
            if (!string.Equals(ValidApiKey, ApiKey))
            {
                HttpContext.Response.StatusCode = 403; // Forbidden
                await HttpContext.Response.WriteAsync("Unauthorized client");
                return;
            }

            await _next(HttpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class ApiKeyMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiKeyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyMiddleware>();
        }
    }
}
