namespace Application.Abstractions.Services
{
    public interface IGoldPriceApiClient
    {
        Task<long> GetLatestPriceAsync(CancellationToken cancellationToken);
    }

}
