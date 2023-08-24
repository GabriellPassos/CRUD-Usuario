using Newtonsoft.Json;
using Refit;

namespace CrudUsuarios
{
    public class CepResponse
    {
        [JsonProperty("cep")]
        public string Cep { get; set; }

        [JsonProperty("logradouro")]
        public string Logradouro { get; set; }
        [JsonProperty("bairro")]
        public string Bairro { get; set; }
        [JsonProperty("localidade")]
        public string Localidade { get; set; }
        [JsonProperty("uf")]
        public string Uf { get; set; }

    }
    public interface ICepService
    {
        [Get("/ws/{cep}/json/")]
        Task<CepResponse> BuscarCepAsync(string? cep);
    }
}
