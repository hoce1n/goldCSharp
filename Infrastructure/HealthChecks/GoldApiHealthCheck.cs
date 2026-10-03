using Application.Abstractions.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks
{
    public class GoldApiHealthCheck : IHealthCheck
    {
        private readonly IGoldPriceApiClient _goldPriceApiClient;
        public GoldApiHealthCheck(IGoldPriceApiClient goldPriceApiClient)
        {
            _goldPriceApiClient = goldPriceApiClient;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var (success, _) = await _goldPriceApiClient.GetLatestPriceAsync(cancellationToken);
                if (success)
                {
                    return HealthCheckResult.Healthy("Gold API is responding normally");
                }

                return HealthCheckResult.Unhealthy("Gold API returned an unsuccessful response");
            } catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy("Gold API is not responnding", ex);
            }
        }
    }
}
