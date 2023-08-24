using CrudUsuarios.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Testes
{
    internal class TestDataHelper
    {
        public static List<Usuario> BuscaFalsaListaUsuarios()
        {
            return new List<Usuario>()
            {
                new Usuario{
                    Nome = "teste",
                    Sobrenome = "silva",
                    Nascimento = new DateTime(2000,10,1),
                    Cpf = "17910227825",
                    Cep ="07417-465"
                },
                                new Usuario{
                    Nome = "teste",
                    Sobrenome = "silva",
                    Nascimento = new DateTime(2000,10,1),
                    Cpf = "22986573860",
                    Cep ="07417-465"
                },
            };
        }
    }
}
