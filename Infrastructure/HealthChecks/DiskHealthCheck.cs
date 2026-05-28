using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Infrastructure.HealthChecks
{
    public class DiskHealthCheck : IHealthCheck
    {
        private readonly string _driveName;
        private readonly long _thresholdInBytes;

        public DiskHealthCheck(string driveName = "C:\\", long thresholdInBytes = 1024L * 1024 * 1024)
        {
            _driveName = driveName;
            _thresholdInBytes = thresholdInBytes;
        }

        public Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var drive = new DriveInfo(_driveName);
                var freeSpace = drive.AvailableFreeSpace;

                if (freeSpace > _thresholdInBytes)
                {
                    return Task.FromResult(HealthCheckResult.Healthy(
                        $"Disk space is sufficient: {freeSpace / (1024 * 1024)} MB free"));
                }

                return Task.FromResult(HealthCheckResult.Degraded(
                    $"Low disk space: {freeSpace / (1024 * 1024)} MB free / threshold: {_thresholdInBytes / (1024 * 1024)} MB"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(HealthCheckResult.Unhealthy(
                    "Failed to check disk space", ex));
            }
        }
    }
}
