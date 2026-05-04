using Application.Abstractions.Messaging;
using Application.Common.Result;

namespace Application.Features.Auth.Query.GetCurrentUser
{
    public record GetCurrentUserQuery() : IQuery<Result<GetCurrentUserResponse>>;
}
