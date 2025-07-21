using AuthApi.Repo;
using AuthApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.RateLimiting;

WebApplicationBuilder Builder = WebApplication.CreateBuilder(args);

Builder.Services.AddControllers();
Builder.Services.AddEndpointsApiExplorer();
//Builder.Services.AddSwaggerGen();
Builder.Services.AddRateLimiter(Options =>
{
    Options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    Options.OnRejected = async (Context, Token) =>
    {
        Context.HttpContext.Response.ContentType = "application/json";
        await Context.HttpContext.Response.WriteAsync(
            "{\"error\":\"Too many login attempts. Please try again later.\"}", cancellationToken: Token);
    };

    Options.AddPolicy("LoginPolicy", Context =>
        RateLimitPartition.GetFixedWindowLimiter(Context, IP =>
            new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
});

ConfigurationManager Configuration = Builder.Configuration;

Builder.Services.AddDbContext<AuthApi.Models.UserManagementDbContext>(Options =>
    Options.UseSqlServer(Configuration.GetConnectionString("DBConnectionString")));

Builder.Services.AddScoped<ILogService, LogService>();
Builder.Services.AddScoped<IAuthService, AuthService>();

WebApplication App = Builder.Build();

//if (App.Environment.IsDevelopment())
//{
//    App.UseSwagger();
//    App.UseSwaggerUI();
//}

App.UseHttpsRedirection();

App.UseRateLimiter();

App.MapControllers();

App.Run();
