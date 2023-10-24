using CrudUsuarios.Data;
using CrudUsuarios.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Testes;

namespace CatalogoAPI.Tests;

public class DbMockHelper
{
    public static async Task PovoarDb(DbHelper dbHelper, bool criar)
    {
        using (var scope = dbHelper.Services.CreateScope())
        {
            var provider = scope.ServiceProvider;
            using (var DbContext = provider.GetRequiredService<AppDbContext>())
            {
                await DbContext.Database.EnsureCreatedAsync();

                if (criar)
                {
                    List<Usuario> usuarios = TestDataHelper.BuscaFalsaListaUsuarios();
                    foreach (var us in usuarios)
                    {
                        await DbContext.Usuarios.AddAsync(us);
                    }
                    await DbContext.SaveChangesAsync();
                }
            }
        }
    }
}