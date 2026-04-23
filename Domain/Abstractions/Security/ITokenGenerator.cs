namespace Domain.Abstractions.Security
{
    public interface ITokenGenerator
    {
        string GenerateRandomToken(int size = 64);
    }
}
