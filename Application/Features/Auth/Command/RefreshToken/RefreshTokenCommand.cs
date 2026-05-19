using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Auth.Command.RefreshToken
{
    public sealed record RefreshTokenCommand(string RefreshToken) : ICommand<Result<RefreshTokenResponse>>
    {
    }
}
