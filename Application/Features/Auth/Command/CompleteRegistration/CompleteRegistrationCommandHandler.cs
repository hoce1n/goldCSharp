using Application.Abstractions.Authentication;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.ValueObjects;
using Domain.Entities.Wallet;

namespace Application.Features.Auth.Command.CompleteRegistration
{
    public sealed class CompleteRegistrationCommandHandler
    : ICommandHandler<CompleteRegistrationCommand, Result<CompleteRegistrationResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUser _currentUser;
        private readonly ITokenService _tokenService;
        private readonly IWalletRepository _walletRepository;

        public CompleteRegistrationCommandHandler(
            IUserRepository userRepository,
            IWalletRepository walletRepository,
            IUnitOfWork unitOfWork,
            ICurrentUser currentUser,
            ITokenService tokenService)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUser = currentUser;
            _tokenService = tokenService;
            _walletRepository = walletRepository;
        }

        public async Task<Result<CompleteRegistrationResponse>> Handle(
            CompleteRegistrationCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(_currentUser.PhoneNumber))
                return Result<CompleteRegistrationResponse>.Failure(
                    Error.Failure(ErrorCodes.Auth.Unauthorized, "کاربر به درستی احراز هویت نشده است."));

            var phoneNumber = new PhoneNumber(_currentUser.PhoneNumber);
            var nationalCode = new NationalCode(request.NationalCode);

            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);

            if (user is null)
                return Result<CompleteRegistrationResponse>.Failure(
                    Error.Failure(ErrorCodes.User.NotFound, "کاربر یافت نشد."));

            user.SetProfile(
                request.FirstName,
                request.LastName,
                request.Birthdate);

            user.SetNationalCode(new NationalCode(request.NationalCode));

            if (!string.IsNullOrWhiteSpace(request.Email))
                user.SetEmail(new Email(request.Email));

            var existingWallet = await _walletRepository.GetByUserIdAsync(user.Id, cancellationToken);
            if (existingWallet is null)
            {
                var wallet = Wallet.Create(user.Id);

                await _walletRepository.AddAsync(wallet, cancellationToken);
            }

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
