using System.Diagnostics;

using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks
{
    public class MemoryHealthCheck : IHealthCheck
    {
        private readonly long _thresholdInBytes;

        public MemoryHealthCheck(long thresholdInBytes = 1024L * 1024 * 500)
        {
            _thresholdInBytes = thresholdInBytes;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var process = Process.GetCurrentProcess();
                var usedMemory = process.WorkingSet64;

                if (usedMemory < _thresholdInBytes)
                {
                    return Task.FromResult(HealthCheckResult.Healthy(
                        $"Memory usage is normal: {usedMemory / (1024 * 1024)} MB"));
                }

                return Task.FromResult(HealthCheckResult.Degraded(
                    $"Memory usage is high: {usedMemory / (1024 * 1024)} MB / threshold: {_thresholdInBytes / (1024 * 1024)} MB"));
            }
            catch (Exception ex) 
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    "Failed to check memory usage", ex));
            }
            throw new NotImplementedException();
        }
    }
}
