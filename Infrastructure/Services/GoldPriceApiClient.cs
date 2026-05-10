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

        public async Task<long> GetLatestPriceAsync(CancellationToken cancellationToken)
        {
            var response = await _httpClient.GetAsync("", cancellationToken);

            response.EnsureSuccessStatusCode();

            var result = await response.Content
                .ReadFromJsonAsync<GoldApiResponse>(cancellationToken);

            var gold18 = result?.Gold
                ?.FirstOrDefault(x => x.Symbol == "IR_GOLD_18K");

            if (gold18 is null)
                throw new Exception("Gold 18K price not found in API response.");

            return gold18.Price;
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
