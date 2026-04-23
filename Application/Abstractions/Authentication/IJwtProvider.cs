namespace Application.Abstractions.Authentication
{
    public interface IJwtProvider
    {
        string GenerateToken(Guid userId, string roles);
    }
}
