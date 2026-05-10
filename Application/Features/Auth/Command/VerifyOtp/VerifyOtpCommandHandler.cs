    using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.Entities.Identity;
using Domain.Services.Identity;
using Domain.ValueObjects;

namespace Application.Features.Auth.Command.VerifyOtp
{
    public sealed class VerifyOtpCommandHandler
        : ICommandHandler<VerifyOtpCommand, Result<VerifyOtpResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IOtpCodeRepository _otpCodeRepository;
        private readonly RefreshTokenDomainService _refreshTokenDomainService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;
        private readonly IUnitOfWork _unitOfWork;
        //private readonly IRoleRepository _roleRepository;

        private const int MaxAttempts = 5;

        public VerifyOtpCommandHandler(
            IUserRepository userRepository,
            IOtpCodeRepository otpCodeRepository,
            RefreshTokenDomainService refreshTokenDomainService,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IUnitOfWork unitOfWork
            //IRoleRepository roleRepository
        )
        {
            _userRepository = userRepository;
            _otpCodeRepository = otpCodeRepository;
            _refreshTokenDomainService = refreshTokenDomainService;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _unitOfWork = unitOfWork;
            //_roleRepository = roleRepository;
        }

        public async Task<Result<VerifyOtpResponse>> Handle(
            VerifyOtpCommand request,
            CancellationToken cancellationToken)
        {
            var phoneNumber = new PhoneNumber(request.PhoneNumber);

            var otp = await _otpCodeRepository
                .GetActiveCodeAsync(phoneNumber, cancellationToken);

            if (otp is null)
            {
                return Result<VerifyOtpResponse>.Failure(
                    Error.Failure(ErrorCodes.OTP.NotFound, "OTP not found."));
            }

            if (otp.IsExpired())
            {
                return Result<VerifyOtpResponse>.Failure(
                    Error.Failure(ErrorCodes.OTP.Expired, "OTP expired."));
            }

            if (otp.IsLockedOut(MaxAttempts))
            {
                return Result<VerifyOtpResponse>.Failure(
                    Error.Failure(ErrorCodes.OTP.TooManyRequests, "تلاش های ناموفق بسیاری داشتید."));
            }

            if (!otp.Code.Equals(request.Code))
            {
                otp.RecordFailedAttempt();
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                return Result<VerifyOtpResponse>.Failure(
                    Error.Failure(ErrorCodes.OTP.Invalid, "Invalid OTP"));
            }

            otp.MarkAsUsed();

            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);

            if (user is null)
            {
                user = new User(phoneNumber);

                user.VerifyPhone();

                await _userRepository.AddAsync(user);
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                user = await _userRepository.GetByIdAsync(user.Id, cancellationToken);
            }
            else
            {
                user.VerifyPhone();
            }

            var accessToken = _tokenService.GenerateAccessToken(user);

            var (rawRefreshToken, refreshTokenEntity) =
                _refreshTokenDomainService.Generate(user, TimeSpan.FromDays(30));

            user.AddRefreshToken(refreshTokenEntity);

            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var response = new VerifyOtpResponse(
                accessToken,
                rawRefreshToken,
                refreshTokenEntity.ExpiresAt,
                user.IsProfileCompleted
            );

            return response;
        }

    }
}
