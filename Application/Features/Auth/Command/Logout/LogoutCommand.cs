using Application.Abstractions.Messaging;
using Application.Common.Result;
using Application.Features.Auth.Command.RefreshToken;

namespace Application.Features.Auth.Command.Logout
{
    public sealed class LogoutCommand : ICommand<Result<LogoutResponse>>
    {
    }
}
