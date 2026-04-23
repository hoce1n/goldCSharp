namespace Application.Abstractions.Authentication
{
    public interface ICurrentUser
    {
        Guid? UserId { get; }
        bool IsAuthenticated { get; }

    }
}
