using CrudUsuarios;
using CrudUsuarios.Data;
using CrudUsuarios.Models;
using CrudUsuarios.Services;
using CrudUsuarios.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Serilog;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<UsuarioService>();
var ConnectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddScoped<IValidator<Usuario>, UsuarioValidation>();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(ConnectionString);
});
builder.Logging.ClearProviders();
builder.Host.UseSerilog((context, config) =>
    config.ReadFrom.Configuration(context.Configuration));
var app = builder.Build();
Log.Information("API Inicializando");
Router.Map(app);
app.Run();
public partial class Program { }

