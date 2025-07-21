using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UserManagementApi.Middlewares;
using UserManagementApi.Models;
using UserManagementApi.Repo;
using UserManagementApi.Services;

WebApplicationBuilder Builder = WebApplication.CreateBuilder(args);

Builder.Services.AddControllers(Options => { Options.Filters.Add(new AuthorizeFilter()); });
Builder.Services.AddEndpointsApiExplorer();
//Builder.Services.AddSwaggerGen(Options =>
//{
//    Options.SwaggerDoc("v1", new OpenApiInfo { Title = "UserManagement API", Version = "v1" });

//    Options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
//    {
//        Description = $"API Key needed to access the endpoints. Example: `{ApiKeyMiddleware.ApiKeyHeaderName}: YOUR_KEY_HERE`",
//        In = ParameterLocation.Header,
//        Name = ApiKeyMiddleware.ApiKeyHeaderName,
//        Type = SecuritySchemeType.ApiKey,
//        Scheme = "ApiKeyScheme"
//    });

//    Options.AddSecurityRequirement(new OpenApiSecurityRequirement
//    {
//        {
//            new OpenApiSecurityScheme
//            {
//                Reference = new OpenApiReference
//                {
//                    Type = ReferenceType.SecurityScheme,
//                    Id = "ApiKey"
//                },
//                In = ParameterLocation.Header,
//                Name = ApiKeyMiddleware.ApiKeyHeaderName
//            },
//            Array.Empty<string>()
//        }
//    });
//});
Builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", Options =>
    {
        Options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidIssuer = "auth-api",
            ValidateAudience = true,
            ValidAudience = "arma-system",
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("X2n+avfQZpK5HVVQQK1Hnp3WysAoIXIE8w6zPfwFq4g")),
            ValidateLifetime = true
        };
    });

ConfigurationManager Configuration = Builder.Configuration;

Builder.Services.AddDbContext<UserManagementDbContext>(Options =>
    Options.UseSqlServer(Configuration.GetConnectionString("DBConnectionString")));

Builder.Services.AddScoped<ILogService, LogService>();
Builder.Services.AddScoped<IPermissionCategoryService, PermissionCategoryService>();
Builder.Services.AddScoped<IPermissionService, PermissionService>();
Builder.Services.AddScoped<IRoleService, RoleService>();
Builder.Services.AddScoped<IRolesPermissionsService, RolesPermissionsService>();
Builder.Services.AddScoped<IUserService, UserService>();

WebApplication App = Builder.Build();

//if (App.Environment.IsDevelopment())
//{
//    App.UseSwagger();
//    App.UseSwaggerUI();
//}

App.UseHttpsRedirection();

App.UseMiddleware<ApiKeyMiddleware>();

App.UseAuthentication();

App.UseAuthorization();

App.MapControllers();

App.Run();
