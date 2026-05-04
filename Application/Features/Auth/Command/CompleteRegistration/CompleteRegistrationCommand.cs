using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Auth.Command.CompleteRegistration
{
    public sealed record CompleteRegistrationCommand(
        string FirstName,
        string LastName,
        string NationalCode,
        DateTime Birthdate,
        string? Email
    ) : ICommand<Result<CompleteRegistrationResponse>>;
}
