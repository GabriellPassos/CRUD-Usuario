using CrudUsuarios.Models;
using CrudUsuarios.Services;
using CrudUsuarios.Validation;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CrudUsuarios
{
    public static class Router
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/", () =>
            {
                Log.Information("GET -> Rota '/'");
                return Results.Ok("Home");
            });

            app.MapPost("usuario", ([FromBody] Usuario usuario, UsuarioService usuarioService) =>
            {
                try
                {
                    Log.Information("POST -> /usuario");
                    Resultado resultado = usuarioService.Novo(usuario).Result;
                    if (!resultado.Sucesso)
                    {
                        return Results.BadRequest(resultado);
                    }
                    return Results.Ok(resultado);
                }
                catch (Exception e)
                {
                    Log.Error("Erro: (POST -> /usuario)", e.Message);
                    return Results.StatusCode(500);
                }
            });
            app.MapGet("usuario/cpf", ([FromQuery] string cpf, UsuarioService usuarioService) =>
            {
                try
                {
                    Log.Information("GET -> /usuario/cpf");
                    Resultado resultado = usuarioService.buscarPorCpf(cpf);
                    if (!resultado.Sucesso)
                    {
                        return Results.BadRequest(resultado);
                    }
                    return Results.Ok(resultado);
                
                }
                catch (Exception e)
                {
                    Log.Information("Erro: (GET -> /usuario/cpf) ", e.Message);
                    return Results.StatusCode(500);
                }
            });
            app.MapGet("usuario/cep", ([FromQuery] string cep, UsuarioService usuarioService) =>
            {
                try
                {
                    Log.Information("GET -> /usuario/cep");
                    Resultado resultado = usuarioService.buscarPorCep(cep);
                    if (!resultado.Sucesso)
                    {
                        return Results.BadRequest(resultado);
                    }
                    return Results.Ok(resultado);
                }
                catch (Exception e)
                {
                    Log.Information("Erro: (GET -> /usuario/cep) ", e.Message);
                    return Results.StatusCode(500);
                }
            });
            app.MapGet("usuarios", (UsuarioService usuarioService) =>
            {
                try
                {
                    Log.Information("GET -> /usuarios");
                    Resultado  resultado = usuarioService.buscarTodos();
                    if (!resultado.Sucesso)
                    {
                        return Results.BadRequest(resultado);
                    }
                    return Results.Ok(resultado);
                }
                catch (Exception e)
                {
                    Log.Information("Erro: (GET -> /usuarios) ", e.Message);
                    return Results.StatusCode(500);
                }
            });
            app.MapPatch("usuario", ([FromBody] Usuario usuario, UsuarioService usuarioService) =>
            {
                try
                {
                    Log.Information("PATCH -> /usuario");
                    Resultado resultado = usuarioService.Editar(usuario).Result;
                    if (!resultado.Sucesso)
                    {
                        return Results.BadRequest(resultado);
                    }
                    return Results.Ok(resultado);
                }
                catch (Exception e)
                {
                    Log.Information("Erro: (PATCH -> /usuario) ", e.Message);
                    return Results.StatusCode(500);
                }
            });
            app.MapDelete("usuario", ([FromQuery] string cpf, UsuarioService usuarioService) =>
            {
                try
                {
                    Log.Information("DELETE -> /usuario");
                    Resultado resultado = usuarioService.Remover(cpf);
                    if (!resultado.Sucesso)
                    {
                        return Results.BadRequest(resultado);
                    }
                    return Results.Ok(resultado);
                }
                catch (Exception e)
                {
                    Log.Information("Erro: (DELETE -> /usuario) ", e.Message);
                    return Results.StatusCode(500);
                }
            });
        }
    }
}
