using AuthApi.Repo;
using AuthApi.Services;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder Builder = WebApplication.CreateBuilder(args);

Builder.Services.AddControllers();
Builder.Services.AddEndpointsApiExplorer();
Builder.Services.AddSwaggerGen();

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

App.MapControllers();

App.Run();
