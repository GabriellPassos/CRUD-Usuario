using CrudUsuarios.Data;
using CrudUsuarios.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Refit;
using System.ComponentModel.DataAnnotations;

namespace CrudUsuarios.Services
{
    public class UsuarioService
    {
        public AppDbContext _context { get; set; }
        private readonly IValidator<Usuario> _validator;
        public UsuarioService(AppDbContext appDbContext, IValidator<Usuario> validator)
        {
            _context = appDbContext;
            _validator = validator;
        }

        public async Task<Resultado> Novo(Usuario usuario)
        {
            var validacao = await _validator.ValidateAsync(usuario);
            if (!validacao.IsValid) { return new Resultado(false, validacao.ToDictionary(), new object[] { usuario }); }
            usuario.Nascimento = usuario.Nascimento.Value.ToUniversalTime();
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();
            return new Resultado(true, validacao.ToDictionary(), new object[] { usuario });


        }
        public Resultado buscarTodos()
        {
            Resultado resultado = new Resultado();
            Dictionary<string, string[]> mensagem = new Dictionary<string, string[]>();
            Usuario[] usuarios = _context.Usuarios.ToArray();
            if (!usuarios.Any())
            {
                resultado.Sucesso = false;
                mensagem.Add("Mensagem", new string[] { "Usuarios não encontrado." });
                resultado.Mensagem = mensagem;
                resultado.Dados = null;
                return resultado;
            }
            resultado.Sucesso = true;
            mensagem.Add("Mensagem", new string[] { "Usuarios encontrados." });
            resultado.Mensagem = mensagem;
            resultado.Dados = usuarios;
            return resultado;
        }
        public Resultado buscarPorCep(string cep)
        {
            Resultado resultado = new Resultado();
            Dictionary<string, string[]> mensagem = new Dictionary<string, string[]>();
            Usuario? usuario = _context.Usuarios.FirstOrDefault(x => x.Cep == cep);
            if (usuario == null)
            {
                resultado.Sucesso = false;
                mensagem.Add("Mensagem", new string[] { "Usuario não encontrado." });
                resultado.Mensagem = mensagem;
                return resultado;
            }
            resultado.Sucesso = true;
            mensagem.Add("Mensagem", new string[] { "Usuario encontrado" });
            resultado.Mensagem = mensagem;
            resultado.Dados.Append(usuario);

            return resultado;
        }
        public Resultado buscarPorCpf(string cpf)
        {
            Resultado resultado = new Resultado();
            Dictionary<string, string[]> mensagem = new Dictionary<string, string[]>();
            Usuario? usuario = _context.Usuarios.FirstOrDefault(x => x.Cpf == cpf);
            if (usuario == null)
            {
                resultado.Sucesso = false;
                mensagem.Add("Mensagem", new string[] { "Usuario não encontrado." });
                resultado.Mensagem = mensagem;
                return resultado;
            }
            resultado.Sucesso = true;
            mensagem.Add("Mensagem", new string[] { "Usuario encontrado" });
            resultado.Mensagem = mensagem;
            resultado.Dados.Append(usuario);
            return resultado;
        }
        public async Task<Resultado> Editar(Usuario usuario)
        {
            var validacao = await _validator.ValidateAsync(usuario);
            if (!validacao.IsValid) { return new Resultado(false, validacao.ToDictionary(), new object[] { usuario }); }
            usuario.Nascimento = usuario.Nascimento.Value.ToUniversalTime();
            _context.Usuarios.Update(usuario);
            _context.SaveChanges();
            return new Resultado(true, validacao.ToDictionary(), new object[] { usuario });
        }
        public Resultado Remover(string cpf)
        {
            Resultado resultado = new Resultado();
            Dictionary<string, string[]> mensagem = new Dictionary<string, string[]>();
            Usuario? usuario = _context.Usuarios.Find(cpf);
            if (usuario == null)
            {
                resultado.Sucesso = false;
                mensagem.Add("Mensagem", new string[] { "Erro ao remover usuario." });
                resultado.Mensagem = mensagem;
                resultado.Dados = null;
                return resultado;
            }
            _context.Remove(usuario);
            _context.SaveChanges();
            resultado.Sucesso = true;
            mensagem.Add("Mensagem", new string[] { "Usuario removido." });
            resultado.Mensagem = mensagem;
            resultado.Dados.Append(usuario);
            return resultado;
        }

    }
}