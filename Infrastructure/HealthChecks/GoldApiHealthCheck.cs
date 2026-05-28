using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks
{
    public class GoldApiHealthCheck : IHealthCheck
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public GoldApiHealthCheck(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("GoldApi");
                var response = await client.GetAsync("https://api.brsapi.ir/Market/Gold_Currency.php?key=BWwtWvrnULuAgmZ6hr6Bh7RwPXbbmhTU", cancellationToken);

                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy("Gold API is responding normally");
                }

                return HealthCheckResult.Unhealthy(
                    $"Gold API returned status code: {response.StatusCode}");
            } catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Gold API is not responnding", ex);
            }
        }
    }
}
