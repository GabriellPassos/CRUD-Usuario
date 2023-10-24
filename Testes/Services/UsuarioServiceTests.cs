using CrudUsuarios.Data;
using CrudUsuarios.Models;
using CrudUsuarios.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using Xunit;


namespace Testes.Services
{
    public class UsuarioServiceTests
    {
        Usuario usuarioTeste;
        public UsuarioServiceTests()
        {
            usuarioTeste = new("teste", "silva", new DateTime(2000, 10, 1), "22986573860", "07417-465");
        }
        [Fact (DisplayName = "Cadastro enviando usuário válido. Sucesso retorna usuário")]
        public void novo_CadastrandoUsuarioValido()
        {
            //ARRANGE
            Usuario usuarioTeste = new("teste", "silva", new DateTime(2000, 10, 1), "22986573860", "07417-465");
            var mockSet = new Mock<DbSet<Usuario>>();
            var contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            contextMock.Setup(m => m.Usuarios).Returns(mockSet.Object);

            //ACT
            UsuarioService usuarioService = new(contextMock.Object);
            Usuario resultado = usuarioService.Novo(usuarioTeste).Result;

            //ASSERT
            mockSet.Verify(x => x.Add(It.IsAny<Usuario>()), Times.Once());
            contextMock.Verify(m => m.SaveChanges(), Times.Once());
            Assert.IsType<Usuario>(resultado);
        }
        [Fact(DisplayName = "Busca uma lista com todos usuários cadastrados.Sucesso retorna lista de usuários")]
        public void buscarTodos_RetornaListaDeUsuarios()
        {
            var mockSet = new Mock<DbSet<Usuario>>();
            var listaUsuariosFalsa = TestDataHelper.BuscaFalsaListaUsuarios().AsQueryable();
            var contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Provider).Returns(listaUsuariosFalsa.Provider);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(listaUsuariosFalsa.Expression);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(listaUsuariosFalsa.ElementType);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(() => listaUsuariosFalsa.GetEnumerator());
            contextMock.Setup(x => x.Usuarios).Returns(mockSet.Object);

            UsuarioService usuarioService = new(contextMock.Object);
            var resultado = usuarioService.buscarTodos();

            Assert.True(listaUsuariosFalsa.Count == resultado.Count);
        }
        [Fact(DisplayName = "Busca um unico usuário pelo CEP. Sucesso retorna usuário")]
        public void buscarPorCep_RetornaUsuarioValido()
        {
            var mockSet = new Mock<DbSet<Usuario>>();
            var listaUsuariosFalsa = TestDataHelper.BuscaFalsaListaUsuarios();
            var contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Provider).Returns(listaUsuariosFalsa.AsQueryable().Provider);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(listaUsuariosFalsa.AsQueryable().Expression);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(listaUsuariosFalsa.AsQueryable().ElementType);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(() => listaUsuariosFalsa.AsQueryable().GetEnumerator());

            contextMock.Setup(x => x.Usuarios).Returns(mockSet.Object);
            UsuarioService usuarioService = new(contextMock.Object);
            var resultado = usuarioService.buscarPorCep(usuarioTeste.Cep);
            Assert.IsType<Usuario>(resultado);
        }
        [Fact(DisplayName = "Busca um unico usuário pelo CPF. Sucesso retorna usuário")]
        public void buscarPorCpf_RetornaUsuarioValido()
        {
            var mockSet = new Mock<DbSet<Usuario>>();
            var listaUsuariosFalsa = TestDataHelper.BuscaFalsaListaUsuarios();
            var contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Provider).Returns(listaUsuariosFalsa.AsQueryable().Provider);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(listaUsuariosFalsa.AsQueryable().Expression);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(listaUsuariosFalsa.AsQueryable().ElementType);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(() => listaUsuariosFalsa.AsQueryable().GetEnumerator());
            contextMock.Setup(x => x.Usuarios).Returns(mockSet.Object);
            UsuarioService usuarioService = new(contextMock.Object);
            var resultado = usuarioService.buscarPorCpf(usuarioTeste.Cpf);

            Assert.IsType<Usuario>(resultado);
        }
        [Fact(DisplayName = "Envia dados válidos para edição de usuário cadastrado. Sucesso retorna usuário")]
        public void editar_UsuarioCadastrado()
        {
            //ARRANGE
            var mockSet = new Mock<DbSet<Usuario>>();
            var contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            var listaUsuariosFalsa = TestDataHelper.BuscaFalsaListaUsuarios().AsQueryable();
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Provider).Returns(listaUsuariosFalsa.Provider);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(listaUsuariosFalsa.Expression);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(listaUsuariosFalsa.ElementType);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(() => listaUsuariosFalsa.GetEnumerator());
            mockSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns((object[] Cpf) =>
            {
                var param = Expression.Parameter(typeof(Usuario), "t");
                var col = Expression.Property(param, "Cpf");
                var body = Expression.Equal(col, Expression.Constant(Cpf[0]));
                var lambda = Expression.Lambda<Func<Usuario, bool>>(body, param);
                return listaUsuariosFalsa.FirstOrDefault(lambda);
            });
            contextMock.Setup(x => x.Usuarios).Returns(mockSet.Object);

            //ACT
            UsuarioService usuarioService = new(contextMock.Object);
            Usuario resultado = usuarioService.Editar(usuarioTeste).Result;

            //ASSERT
            contextMock.Verify(m => m.SaveChanges(), Times.Once());
            Assert.IsType<Usuario>(resultado);
        }
        [Fact(DisplayName = "Envia dados válidos para remoção de usuário cadastrado. Sucesso remove elemento")]
        public void remover_UsuarioCadastrado()
        {
            var listaUsuariosFalsa = TestDataHelper.BuscaFalsaListaUsuarios();
            int comprimentoInicial = listaUsuariosFalsa.Count;
            var mockSet = new Mock<DbSet<Usuario>>();
            var contextMock = new Mock<AppDbContext>(new DbContextOptions<AppDbContext>());
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Provider).Returns(listaUsuariosFalsa.AsQueryable().Provider);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.Expression).Returns(listaUsuariosFalsa.AsQueryable().Expression);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.ElementType).Returns(listaUsuariosFalsa.AsQueryable().ElementType);
            mockSet.As<IQueryable<Usuario>>().Setup(m => m.GetEnumerator()).Returns(() => listaUsuariosFalsa.AsQueryable().GetEnumerator());
            mockSet.Setup(x => x.Find(It.IsAny<object[]>())).Returns((object[] Cpf) =>
            {
                var param = Expression.Parameter(typeof(Usuario), "t");
                var col = Expression.Property(param, "Cpf");
                var body = Expression.Equal(col, Expression.Constant(Cpf[0]));
                var lambda = Expression.Lambda<Func<Usuario, bool>>(body, param);
                return listaUsuariosFalsa.AsQueryable().FirstOrDefault(lambda);
            });
            contextMock.Setup(m => m.Remove(It.IsAny<Usuario>())).Callback<Usuario>((entity) => listaUsuariosFalsa.Remove(entity));
            contextMock.Setup(x => x.Usuarios).Returns(mockSet.Object);

            UsuarioService usuarioService = new(contextMock.Object);
            usuarioService.Remover(usuarioTeste.Cpf);
            int comprimentoFinal = listaUsuariosFalsa.Count;

            mockSet.Verify(x => x.Find(It.IsAny<object[]>()), Times.Once());
            contextMock.Verify(x => x.Remove(It.IsAny<Usuario>()), Times.Once());
            contextMock.Verify(m => m.SaveChanges(), Times.Once());
            Assert.True(comprimentoInicial > comprimentoFinal);
        }
    }
}
