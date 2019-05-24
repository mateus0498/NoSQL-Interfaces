// Autor: Mateus Ferreira

using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace elasticSearch.Services
{
    public class RestService
    {
        HttpClient _client;
        readonly string BaseAddress = "http://localhost:9200";

        public RestService ()
        {
            _client = new HttpClient ();
        }

        public async Task<bool> TestarConexao()
        {
            var uri = new Uri(BaseAddress);
            
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        public async Task<bool> ListarIndices()
        {
            var uri = new Uri(BaseAddress + "/_cat/indices?v");
            
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode){
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                return true;
            }

            return false;
        }

        public async Task<bool> CriarIndice(string nome)
        {
            var uri = new Uri(BaseAddress + "/" + nome + "?pretty");

            var response = await _client.PutAsync(uri, null);

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        public async Task<bool> DeletarIndice(string nome)
        {
            var uri = new Uri(BaseAddress + "/" + nome + "?pretty");

            var response = await _client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
                return true;

            return false;
        }

        public async Task<bool> AdicionarDocumento(string nomeindice, int id, string pathToJson)
        {
            var uri = new Uri(BaseAddress + "/" + nomeindice + "/_doc/" + id + "?pretty");

            try
            {
                var text = System.IO.File.ReadAllText(pathToJson);
                var content = new StringContent(text, Encoding.UTF8, "application/json");

                var response = await _client.PutAsync(uri, content);

                if (response.IsSuccessStatusCode)
                    return true;
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("ERRO: arquivo inexistente\n");
            }

            return false;
        }

    }
}