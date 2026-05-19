using Application.Abstractions.Services;
using System.Net.Http.Json;

namespace Infrastructure.Services
{
    public sealed class GoldPriceApiClient : IGoldPriceApiClient
    {
        private readonly HttpClient _httpClient;

        public GoldPriceApiClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<(bool success, long price)> GetLatestPriceAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync("", cancellationToken);

                if(!response.IsSuccessStatusCode)
                    return (false, 0);

                var result = await response.Content.ReadFromJsonAsync<GoldApiResponse>(cancellationToken);

                var gold = result?.Gold?.FirstOrDefault(x => x.Symbol == "IR_GOLD_18K");

                if (gold is null)
                    return (false, 0);

                return (true, gold.Price);
            }
            catch
            {
                return (false, 0);
            }
        }
    }
    public sealed class GoldApiResponse
    {
        public List<GoldItem> Gold { get; set; } = new();
    }
    public sealed class GoldItem
    {
        public string Symbol { get; set; } = default!;
        public long Price { get; set; }
        public long Time_Unix { get; set; }
    }

}
