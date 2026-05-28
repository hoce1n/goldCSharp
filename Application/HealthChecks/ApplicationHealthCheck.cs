using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Application.HealthChecks
{
    public class ApplicationHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            var isHealthy = true;

            if (isHealthy)
            {
                return Task.FromResult(HealthCheckResult.Healthy("Application is healthy"));
            }

            return Task.FromResult(HealthCheckResult.Unhealthy("Application is unhealthy"));
        }
    }
}
