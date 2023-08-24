using CrudUsuarios.Models;
using CrudUsuarios.Services;
using Microsoft.AspNetCore.Mvc;


namespace CrudUsuarios
{
    public static class Router
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/", () =>
            {
                return Results.BadRequest("Odsadasdas");
            });

            app.MapPost("usuario", ([FromBody] Usuario usuario, UsuarioService usuarioService) =>
            {
                try
                {
                    Usuario novoUsuario = usuarioService.Novo(usuario).Result;
                    if (novoUsuario != null)
                    {
                        return Results.Ok(novoUsuario);
                    }
                }
                catch (Exception e)
                {

                    return Results.BadRequest(e.Message);

                }
                return Results.BadRequest();
            });
            app.MapGet("usuario/cpf", ([FromQuery] string cpf, UsuarioService usuarioService) =>
            {
                try
                {
                    Usuario usuario = usuarioService.buscarPorCpf(cpf);

                    return Results.Ok(usuario);
                }
                catch (Exception e)
                {

                    return Results.BadRequest(e.Message);
                }
            });
            app.MapGet("usuario/cep", ([FromQuery] string cep, UsuarioService usuarioService) =>
            {
                try
                {
                    Usuario usuario = usuarioService.buscarPorCep(cep);

                    return Results.Ok(usuario);
                }
                catch (Exception e)
                {

                    return Results.BadRequest(e.Message);
                }
            });
            app.MapGet("usuarios", (UsuarioService usuarioService) =>
            {
                return Results.Ok(usuarioService.buscarTodos());
            });
            app.MapPatch("usuario", ([FromBody] Usuario usuario, UsuarioService usuarioService) =>
            {
                try
                {
                    Usuario usuarioEditado = usuarioService.Editar(usuario).Result;
                    if (usuarioEditado != null)
                    {
                        return Results.Ok(usuarioEditado);
                    }
                }
                catch (Exception e)
                {

                    return Results.BadRequest(e.Message);

                }
                return Results.BadRequest();
            });
            app.MapDelete("usuario", ([FromQuery] string cpf, UsuarioService usuarioService) =>
            {
                try
                {
                    usuarioService.Remover(cpf);
                    return Results.Ok("Usuário removido.");
                }
                catch (Exception e)
                {
                    return Results.BadRequest(e.Message);
                }
            });
        }
    }
}
