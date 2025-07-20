namespace UserManagementApi.Middlewares
{
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
}
