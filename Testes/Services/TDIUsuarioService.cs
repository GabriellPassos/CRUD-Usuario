using CatalogoAPI.Tests;
using CrudUsuarios.Models;
using Xunit;
using System.Net;
using System.Net.Http.Json;

namespace Testes.Services
{
    public class TDIUsuarioService
    {
        [Fact]
        public async Task novo_CadastroUsuarioValido()
        {
            await using var dbHelper = new DbHelper();
            await DbMockHelper.PovoarDb(dbHelper, true);
            var url = "/usuario";
            Usuario usuario = new Usuario(
                "teste",
                "silva",
                new DateTime(2000, 10, 1),
                "11716110050",
                "07417-465"
                );
            var cliente = dbHelper.CreateClient();
            var resultado = await cliente.PostAsJsonAsync(url, usuario);
            var usuarios = await cliente.GetFromJsonAsync<List<Usuario>>("/usuarios");
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
            Assert.NotNull(usuarios);
            Assert.True(usuarios.Count == 3);
        }
        [Fact]
        public async Task buscar_TodosOsUsuarios()
        {
            await using var dbHelper = new DbHelper();
            await DbMockHelper.PovoarDb(dbHelper, true);
            var url = "/usuarios";
            var cliente = dbHelper.CreateClient();
            var resultado = await cliente.GetAsync(url);
            var usuarios = await cliente.GetFromJsonAsync<List<Usuario>>(url);

            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
            Assert.NotNull(usuarios);
            Assert.Equal(2, usuarios.Count);
        }
        [Fact]
        public async Task buscar_UsuarioPorCpf()
        {
            await using var dbHelper = new DbHelper();
            await DbMockHelper.PovoarDb(dbHelper, true);
            var url = "usuario/cpf?cpf=17910227825";
            var cliente = dbHelper.CreateClient();
            var resultado = await cliente.GetAsync(url );
            var usuario = await cliente.GetFromJsonAsync<Usuario>(url);

            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
            Assert.NotNull(usuario);
        }
        [Fact]
        public async Task editar_UsuarioComDadosValidos()
        {
            await using var dbHelper = new DbHelper();
            await DbMockHelper.PovoarDb(dbHelper, true);
            var url = "usuario";
            Usuario usuario = new Usuario(
                "nomeEditado",
                "sobrenomeEditado",
                new DateTime(2011, 10, 1).ToUniversalTime(),
                "17910227825",
                "07417-465"
                );
            var cliente = dbHelper.CreateClient();
            var resultado = await cliente.PatchAsJsonAsync(url, usuario);
            var usuarioEditado = await cliente.GetFromJsonAsync<Usuario>("usuario/cpf?cpf=17910227825");
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
            Assert.Equivalent(usuario, usuarioEditado);
        }
        [Fact]
        public async Task remover_Usuario()
        {
            await using var dbHelper = new DbHelper();
            await DbMockHelper.PovoarDb(dbHelper, true);
            var url = "usuario?cpf=22986573860";
            var cliente = dbHelper.CreateClient();
            var resultado = await cliente.DeleteAsync(url);
            var usuarios = await cliente.GetFromJsonAsync<List<Usuario>>("usuarios");

            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
            Assert.NotNull(usuarios);
            Assert.True(usuarios.Count == 1);
        }
    }
}
