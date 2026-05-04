namespace Application.Features.Auth.Command.CompleteRegistration
{
    public sealed record CompleteRegistrationResponse(
        string AccessToken,
        bool IsProfileCompleted,
        string FirstName,
        string LastName,
        string NationalCode,
        DateTime BirthDate,
        string? Email
    );
}
