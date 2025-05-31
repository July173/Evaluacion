using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Data.Interfaces;
using Business.Interfaces;
using Business.Implements;
using Web.ServiceExtension;
using FluentValidation;
using FluentValidation.AspNetCore;
using Data.Implements.BaseDate;
using Data.Implements.BaseData;
using Data.Implements;
using Entity.Dtos.CityDto;
using Entity.Dtos.ClientDto;
using Utilities.Mappers.Profiles;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);

builder.Services.AddValidatorsFromAssemblyContaining(typeof(Program));
builder.Services.AddSingleton<IValidatorFactory>(sp =>
    new ServiceProviderValidatorFactory(sp));

// Swagger
builder.Services.AddSwaggerDocumentation();

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register generic repositories and business logic
builder.Services.AddScoped(typeof(IBaseModelData<>), typeof(BaseModelData<>));
builder.Services.AddScoped(typeof(IBaseBusiness<,>), typeof(BaseBusiness<,>));

// Register Author-specific services
builder.Services.AddScoped<IAuthorData, AuthorData>();
builder.Services.AddScoped<IAuthorBusiness, AuthorBusiness>();

// Register Book-specific services
builder.Services.AddScoped<IBookData, BookData>();
builder.Services.AddScoped<IBookBusiness, BookBusiness>();

builder.Services.AddAutoMapper(typeof(AuthorProfile));
builder.Services.AddAutoMapper(typeof(BookProfile));



var app = builder.Build();

// Swagger (solo en desarrollo)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Sistema de Gestión v1");
        c.RoutePrefix = string.Empty;
    });
}

// Usa la política de CORS registrada en ApplicationServiceExtensions
app.UseCors("AllowSpecificOrigins");

app.UseHttpsRedirection();

// Autenticación y autorización
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
