using System.Net.Http.Json;

namespace SlojPoslovneLogike.Ogranicenja
{
    public class CitacPravila
    {
        private readonly HttpClient _httpClient;
        private const string UrlServisa = "http://localhost:5231/api/PravilaRest";

        public CitacPravila(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<decimal> DohvatiProcenatPopusta()
        {
            try
            {
                var pravila = await _httpClient.GetFromJsonAsync<PravilaModel>(UrlServisa);
                return pravila?.ProcenatPopusta ?? 10;
            }
            catch
            {
                return 10;
            }
        }
    }

    public class PravilaModel
    {
        public decimal ProcenatPopusta { get; set; }
        public string Opis { get; set; } = string.Empty;
    }
}