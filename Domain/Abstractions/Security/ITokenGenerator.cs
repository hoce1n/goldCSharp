namespace Domain.Abstractions.Security
{
    public interface ITokenGenerator
    {
        string GenerateRandomToken();
    }
}
