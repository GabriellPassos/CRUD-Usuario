using CrudUsuarios;
using CrudUsuarios.Data;
using CrudUsuarios.Services;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<UsuarioService>();
var ConnectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(ConnectionString);
});
var app = builder.Build();

Router.Map(app);
app.Run();
