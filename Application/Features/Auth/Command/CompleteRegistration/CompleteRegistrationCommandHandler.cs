using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.ValueObjects;

namespace Application.Features.Auth.Command.CompleteRegistration
{
    public sealed class CompleteRegistrationCommandHandler
    : ICommandHandler<CompleteRegistrationCommand, Result<CompleteRegistrationResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly ITokenService _tokenService;

        public CompleteRegistrationCommandHandler(
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _tokenService = tokenService;
        }

        public async Task<Result<CompleteRegistrationResponse>> Handle(
            CompleteRegistrationCommand request,
            CancellationToken cancellationToken)
        {
            var phoneNumber = new PhoneNumber(_currentUser.PhoneNumber);

            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);

            if (user is null)
                return Error.Failure(ErrorCodes.User.NotFound, "User not found.");

            user.SetProfile(
                request.FirstName,
                request.LastName,
                request.Birthdate);

            user.SetNationalCode(new NationalCode(request.NationalCode));

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.SetEmail(new Email(request.Email));

            await _userRepository.UpdateAsync(user, cancellationToken);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var accessToken = _tokenService.GenerateAccessToken(user);

            var response = new CompleteRegistrationResponse(
                accessToken,
                user.IsProfileCompleted,
                user.FirstName,
                user.LastName,
                user.NationalCode.Value,
                user.BirthDate.Value,
                user.Email?.Value
            );

            return response;
        }
    }
}
