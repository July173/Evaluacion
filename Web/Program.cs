using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using Entity.Context;
using Data.Interfaces;
using Data.Implements.RolData;
using Data.Implements.RolUserData;
using Data.Implements.UserDate;
using Business.Interfaces;
using Business.Implements;
using Utilities.Interfaces;
using Utilities.Helpers;
using Utilities.Mail;
using Utilities.Jwt;
using Web.ServiceExtension;
using FluentValidation;
using FluentValidation.AspNetCore;
using Business.Services;
using Data.Implements.BaseDate;
using Data.Implements.BaseData;
using Data.Implements.Security;
using Data.Implements.Others;
using Utilities.Interfaces.Security;
using Utilities.Helpers.Security;
using Business.Implements.Security;
using Business.Implements.OthersDates;



var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration); // 


builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddSingleton<IValidatorFactory>(sp =>
    new ServiceProviderValidatorFactory(sp));

// Add Swagger documentation using extension method
builder.Services.AddSwaggerDocumentation();

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDbContext<AuditDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("AuditConnection")));

builder.Services.AddScoped<IDbContextFactory, DbContextFactory>();

// Configure email service
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));


// Configure JWT
builder.Services.AddScoped<IJwtGenerator, GenerateTokenJwt>();

// Register generic repositories and business logic


// Existing code remains unchanged
builder.Services.AddScoped(typeof(IBaseModelData<>), typeof(BaseModelData<>));

builder.Services.AddScoped(typeof(IBaseBusiness<,>), typeof(BaseBusiness<,>));

// Register User-specific services
builder.Services.AddScoped<IUserData, UserData>();
builder.Services.AddScoped<IUserBusiness, UserBusiness>();

// Register Role-specific services
builder.Services.AddScoped<IRolData, RolData>();
builder.Services.AddScoped<IRolBusiness, RolBusiness>();

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtService, JwtService>();

// Register RoleUser-specific services
builder.Services.AddScoped<IRolUserData, RolUserData>();
builder.Services.AddScoped<IRoleUserBusiness, RoleUserBusiness>();

builder.Services.AddScoped<IPersonData, PersonData>();
builder.Services.AddScoped<IPersonBusiness, PersonBusiness>();

builder.Services.AddScoped<IFormData, FormData>();
builder.Services.AddScoped<IFormBusiness, FormBusiness>();

builder.Services.AddScoped<IModuleData, ModuleData>();
builder.Services.AddScoped<IModuleBusiness, ModuleBusiness>();

builder.Services.AddScoped<IFormModuleData, FormModuleData>();
builder.Services.AddScoped<IFormModuleBusiness, FormModuleBusiness>();

builder.Services.AddScoped<IPermissionData, PermissionData>();
builder.Services.AddScoped<IPermissionBusiness, PermissionBusiness>();

builder.Services.AddScoped<IRolFormPermissionData, RolFormPermissionData>();
builder.Services.AddScoped<IRolFormPermissionBusiness, RolFormPermissionBusiness>();


builder.Services.AddScoped<IProviderData, ProviderData>();
builder.Services.AddScoped<IProviderBusiness, ProviderBusiness>();

builder.Services.AddScoped<ICityData, CityData>();
builder.Services.AddScoped<ICityBusiness, CityBusiness>();


builder.Services.AddScoped<ICountryData, CountryData>();
builder.Services.AddScoped<ICountryBusiness, CountryBusiness>();

builder.Services.AddScoped<IDepartmentData, DepartmentData>();
builder.Services.AddScoped<IDepartmentBusiness, DeparmentBusiness>();

builder.Services.AddScoped<INeighborhoodData, NeighborhoodData>();
builder.Services.AddScoped<INeighborhoodBusiness, NeighborhoodBusiness>();


builder.Services.AddScoped<IClientData, ClientData>();
builder.Services.AddScoped<IClientBusiness, ClientBusiness>();

builder.Services.AddScoped<IEmployeeData, EmployeeData>();
builder.Services.AddScoped<IEmployeeBusiness, EmployeeBusiness>();


builder.Services.AddScoped<AuditBusiness>();



// Register utility helpers
builder.Services.AddScoped<IGenericIHelpers, GenericHelpers>();
builder.Services.AddScoped<IDatetimeHelper, DatetimeHelper>();
builder.Services.AddScoped<IPasswordHelper, PasswordHelper>();
builder.Services.AddScoped<IAuthHeaderHelper, AuthHeaderHelper>();
builder.Services.AddScoped<IRoleHelper, RoleHelper>();
builder.Services.AddScoped<IUserHelper, UserHelper>();



builder.Services.AddScoped<IValidationHelper, ValidationHelper>();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    var key = Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"] ?? throw new InvalidOperationException("JWT:Key is not configured"));
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!.Split(";");
{
  

    var app = builder.Build();

    // Configure the HTTP request pipeline
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Gestión v1");
            c.RoutePrefix = string.Empty; // Para servir Swagger UI en la raíz
        });
        
    }

  

    // Enable CORS
    app.UseCors();

    app.UseHttpsRedirection();

    // Add authentication & authorization
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    // Inicializar base de datos y aplicar migraciones
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var dbContext = services.GetRequiredService<ApplicationDbContext>();
            

            var logger = services.GetRequiredService<ILogger<Program>>();

            // Aplicar migraciones (esto crea la BD si no existe y aplica todas las migraciones)
            dbContext.Database.Migrate();
            logger.LogInformation("Base de datos verificada y migraciones aplicadas exitosamente.");
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Ocurrió un error durante la migración de la base de datos.");
        }
    }

    app.Run();


}
