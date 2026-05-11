using Application.Abstractions.Services;
using Application.Services.Catalog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Services
{
    public sealed class MarketPriceWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public MarketPriceWorker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceProvider.CreateScope();

                var service = scope.ServiceProvider
                    .GetRequiredService<IMarketPriceService>();

                await service.RefreshFromExternalApiAsync(stoppingToken);
                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}
