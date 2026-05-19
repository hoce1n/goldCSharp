using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Result;
using Domain.Common.Errors;

namespace Application.Features.Auth.Query.GetCurrentUser
{
    public class GetCurrentUserQueryHandler
        : IQueryHandler<GetCurrentUserQuery, Result<GetCurrentUserResponse>>
    {

        private readonly ICurrentUser _currentUser;
        private readonly IUserRepository _userRepository;

        public GetCurrentUserQueryHandler(
            ICurrentUser currentUser, 
            IUserRepository userRepository)
        {
            _currentUser = currentUser;
            _userRepository = userRepository;
        }

        public async Task<Result<GetCurrentUserResponse>> Handle(
            GetCurrentUserQuery request,
            CancellationToken cancellationToken)
        {
            if (!_currentUser.IsAuthenticated || _currentUser.UserId is null)
                return Result<GetCurrentUserResponse>.Failure(
                    Error.Failure(ErrorCodes.User.Unauthorized, "لطفا ابتدا وارد شوید."));

            var user = await _userRepository.GetByIdAsync(_currentUser.UserId.Value, cancellationToken);

            if (user is null)
                return Result<GetCurrentUserResponse>.Failure(
                    Error.Failure(ErrorCodes.User.NotFound, "کاربر یافت نشد."));

            var response = new GetCurrentUserResponse
            {
                Id = user.Id,
                PhoneNumber = user.PhoneNumber.Value,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = user.Role,
                NationalCode = user.NationalCode?.Value ?? string.Empty,
                Birthdate = user.BirthDate.ToString() ?? string.Empty,
                Email = user.Email?.Value ?? string.Empty,
                VerificationLevel = user.VerificationLevel.ToString(),
                IsProfileCompleted = user.IsProfileCompleted,
            };

            return response;
        }
    }
}
