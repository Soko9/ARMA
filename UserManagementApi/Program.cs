using Microsoft.EntityFrameworkCore;
using UserManagementApi.Models;
using UserManagementApi.Repo;
using UserManagementApi.Services;

var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<UserManagementDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DBConnectionString")));

builder.Services.AddScoped<ILogService, LogService>();
builder.Services.AddScoped<IPermissionCategoryService, PermissionCategoryService>();
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IRolesPermissionsService, RolesPermissionsService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
