using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Auth.Command.RefreshToken
{
    public sealed class RefreshTokenCommand : ICommand<Result<RefreshTokenResponse>>
    {
    }
}
