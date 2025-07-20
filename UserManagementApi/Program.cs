using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UserManagementApi.Middlewares;
using UserManagementApi.Models;
using UserManagementApi.Repo;
using UserManagementApi.Services;

var Builder = WebApplication.CreateBuilder(args);

ConfigurationManager Configuration = Builder.Configuration;

Builder.Services.AddDbContext<UserManagementDbContext>(Options =>
    Options.UseSqlServer(Configuration.GetConnectionString("DBConnectionString")));

Builder.Services.AddScoped<ILogService, LogService>();
Builder.Services.AddScoped<IPermissionCategoryService, PermissionCategoryService>();
Builder.Services.AddScoped<IPermissionService, PermissionService>();
Builder.Services.AddScoped<IRoleService, RoleService>();
Builder.Services.AddScoped<IRolesPermissionsService, RolesPermissionsService>();
Builder.Services.AddScoped<IUserService, UserService>();

Builder.Services.AddControllers(/*Options => { Options.Filters.Add(new AuthorizeFilter()); }*/);
Builder.Services.AddEndpointsApiExplorer();
Builder.Services.AddSwaggerGen(Options =>
{
    Options.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManagement API", Version = "v1" });

    Options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = $"API Key needed to access the endpoints. Example: `{ApiKeyMiddleware.ApiKeyHeaderName}: YOUR_KEY_HERE`",
        In = ParameterLocation.Header,
        Name = ApiKeyMiddleware.ApiKeyHeaderName,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "ApiKeyScheme"
    });

    Options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "ApiKey"
                },
                In = ParameterLocation.Header,
                Name = ApiKeyMiddleware.ApiKeyHeaderName
            },
            Array.Empty<string>()
        }
    });
});

var App = Builder.Build();

if (App.Environment.IsDevelopment())
{
    App.UseSwagger();
    App.UseSwaggerUI();
}

App.UseHttpsRedirection();

App.UseMiddleware<ApiKeyMiddleware>();

//App.UseAuthentication();

//App.UseAuthorization();

App.MapControllers();

App.Run();
