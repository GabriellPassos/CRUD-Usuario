using CrudUsuarios.Data;
using CrudUsuarios.Models;
using CrudUsuarios.Services;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Refit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CrudUsuarios.Validation
{
    public class UsuarioValidation : AbstractValidator<Usuario>
    {
        private AppDbContext _context;
        public UsuarioValidation(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public UsuarioValidation()
        {
            RuleFor(x => x.Nome).NotNull().NotEmpty().MinimumLength(4);
            RuleFor(x => x.Sobrenome).NotNull().NotEmpty().MinimumLength(4);
            RuleFor(x => x.Nascimento).NotNull().NotEmpty().LessThan(DateTime.Now).GreaterThan(new DateTime(1900, 1, 1).ToUniversalTime());
            RuleFor(x => x.Cpf).NotNull().NotEmpty().Must(CpfService.Validar).WithMessage("CPF inváido");
            RuleFor(x => x.Cpf).NotNull().NotEmpty().MustAsync(async
                (cpf, cancelation) =>
            {
                return !_context.Usuarios.AnyAsync(x => x.Cpf == cpf).Result;
            }).WithMessage("Usuario cadastrado");
            RuleFor(x => x.Cep).NotNull().NotEmpty().MustAsync(async (cep, cancelation) => { return ValidarCep(cep).Result; }).WithMessage("CEP inváido");
        }
        async private Task<bool> ValidarCep(string cep)
        {
            var cepClient = RestService.For<ICepService>("https://viacep.com.br/");
            CepResponse endereco;
            endereco = await cepClient.BuscarCepAsync(cep);
            return endereco.Cep == cep;
        }

    }

}
