namespace CrudUsuarios.Models
{
    public class Resultado
    {
        public bool Sucesso { get; set; }
        public IDictionary<string, string[]> Mensagem { get; set; }
        public Object[] Dados { get; set; }

        public Resultado()
        {
                
        }

        public Resultado(bool sucesso, IDictionary<string, string[]> mensagem, Object[] dados)
        {
            Sucesso = sucesso;
            Mensagem = mensagem;
            Dados = dados;
        }
    }
}
