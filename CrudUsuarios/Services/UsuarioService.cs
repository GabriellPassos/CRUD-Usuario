using CrudUsuarios.Data;
using CrudUsuarios.Models;
using Refit;
using System.ComponentModel.DataAnnotations;

namespace CrudUsuarios.Services
{
    public class UsuarioService
    {
        public AppDbContext _context { get; set; }
        public UsuarioService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }

        public async Task<Usuario> Novo(Usuario usuario)
        {
            try
            {
                ICollection<ValidationResult> errors = new List<ValidationResult>();
                if (usuario != null)
                {
                    //Validacao
                    //Usuario
                    Validator.TryValidateObject(usuario, new ValidationContext(usuario), errors, true);
                    if (errors.Count == 0)
                    {
                        usuario.Nascimento = usuario.Nascimento.Value.ToUniversalTime();
                        //CEP
                        var cepClient = RestService.For<ICepService>("https://viacep.com.br/");
                        CepResponse endereco;
                        endereco = await cepClient.BuscarCepAsync(usuario.Cep);
                        if (endereco.Cep != usuario.Cep)
                        {
                            errors.Add(new ValidationResult("Cep inválido"));
                            throw new Exception("Cep inválido");
                        }
                        //CPF
                        bool cpfValido = CpfService.Validar(usuario.Cpf);
                        if (!cpfValido)
                        {
                            errors.Add(new ValidationResult("Cpf inválido"));
                            throw new Exception("Cpf inválido");
                        }
                    }
                    if (errors.Count > 0)
                    {
                        string fraseErro = "";
                        foreach (var error in errors)
                        {
                            fraseErro += error + " \n ";
                        }
                        throw new Exception(fraseErro);
                    }

                    _context.Usuarios.Add(usuario);
                    _context.SaveChanges();
                    return usuario;
                }

                throw new Exception("Dados do usuário inválidos");
            }
            catch (Exception e)
            {
                if (e.InnerException != null)
                {
                    if (e.InnerException.Message.Contains("duplicate key value violates unique constraint"))
                    {
                        throw new Exception("CPF inválido, usuário já cadastrado");
                    }
                }
                throw new Exception(e.Message);
            }
        }
        public IList<Usuario> buscarTodos()
        {
            return _context.Usuarios.ToList();
        }
        public Usuario buscarPorCep(string cep)
        {
            Usuario? usuario = _context.Usuarios.FirstOrDefault(x => x.Cep == cep);
            if (usuario != null)
            {
                return usuario;
            }
            throw new Exception("Cep não encontrado.");
        }
        public Usuario buscarPorCpf(string cpf)
        {
            Usuario? usuario = _context.Usuarios.FirstOrDefault(x => x.Cpf == cpf);
            if (usuario != null)
            {
                return usuario;
            }
            throw new Exception("Cpf não encontrado.");
        }
        public async Task<Usuario> Editar(Usuario usuario)
        {
            try
            {
                ICollection<ValidationResult> errors = new List<ValidationResult>();
                if (usuario != null)
                {
                    Usuario usuarioCadastrado = _context.Usuarios.Find(usuario.Cpf) ??
                        throw new Exception("Usuário não encontrado. CPF não cadastrado.");
                    //Validacao
                    //Usuario
                    Validator.TryValidateObject(usuario, new ValidationContext(usuario), errors, true);
                    if (errors.Count == 0)
                    {
                        usuario.Nascimento = usuario.Nascimento.Value.ToUniversalTime();
                        //CEP
                        var cepClient = RestService.For<ICepService>("https://viacep.com.br/");
                        CepResponse endereco;
                        endereco = await cepClient.BuscarCepAsync(usuario.Cep);
                        if (endereco.Cep != usuario.Cep)
                        {
                            errors.Add(new ValidationResult("Cep inválido"));
                            throw new Exception("Cep inválido");
                        }

                    }
                    if (errors.Count > 0)
                    {
                        string fraseErro = "";
                        foreach (var error in errors)
                        {
                            fraseErro += error + " \n ";
                        }
                        throw new Exception(fraseErro);
                    }

                    usuarioCadastrado = usuario;
                    _context.SaveChanges();
                    return usuario;
                }
                throw new Exception("Dados do usuário inválidos");
            }
            catch (Exception e)
            {

                throw new Exception(e.Message);
            }
        }
        public void Remover(string cpf)
        {
            Usuario? usuario = _context.Usuarios.Find(cpf) ?? throw new Exception("Usuário não encontrado");
            _context.Remove(usuario);
            _context.SaveChanges();
        }

    }
}