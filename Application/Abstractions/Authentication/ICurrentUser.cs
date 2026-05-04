namespace Application.Abstractions.Authentication
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }
        string? PhoneNumber { get; }
        bool IsAuthenticated { get; }
        string? Role {  get; }
    }
}
