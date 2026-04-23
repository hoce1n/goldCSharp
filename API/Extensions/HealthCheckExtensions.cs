using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API.Extensions
{
    public static class HealthCheckExtensions
    {
        public static IServiceCollection AddApplicationHealthChecks(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddCheck("self", () => HealthCheckResult.Healthy(), tags: new[] { "liveness" })
                .AddSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    name: "sql",
                    tags: new[] { "readiness" });

            return services;
        }
    }
}
