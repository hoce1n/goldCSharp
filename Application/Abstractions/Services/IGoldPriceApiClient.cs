namespace Application.Abstractions.Services
{
    public interface IGoldPriceApiClient
    {
        Task<(bool success, long price)> GetLatestPriceAsync(CancellationToken cancellationToken);
    }

}
