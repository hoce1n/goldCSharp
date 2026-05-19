using Application.Abstractions.Configuration;
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
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOtpSettings _otpSettings;
        private readonly IAuthSetting _authSetting;

        public VerifyOtpCommandHandler(
            IUserRepository userRepository,
            IOtpCodeRepository otpCodeRepository,
            RefreshTokenDomainService refreshTokenDomainService,
            IRefreshTokenRepository refreshTokenRepository,
            ITokenService tokenService,
            IDateTimeProvider dateTimeProvider,
            IUnitOfWork unitOfWork,
            IOtpSettings otpSettings,
            IAuthSetting authSetting
        )
        {
            _userRepository = userRepository;
            _otpCodeRepository = otpCodeRepository;
            _refreshTokenDomainService = refreshTokenDomainService;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
            _otpSettings = otpSettings;
            _authSetting = authSetting;
        }

        public async Task<Result<VerifyOtpResponse>> Handle(
            VerifyOtpCommand request,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;
            var expiry = TimeSpan.FromDays(_authSetting.RefreshTokenExpiryDays);
            var phoneNumber = new PhoneNumber(request.PhoneNumber);

            var otp = await _otpCodeRepository
                .GetActiveCodeAsync(phoneNumber, cancellationToken);

            if (otp is null)
            {
                return Result<VerifyOtpResponse>.Failure(
                    Error.Failure(ErrorCodes.OTP.NotFound, "کد تایید یافت نشد."));
            }

            if (otp.IsExpired(now))
            {
                return Result<VerifyOtpResponse>.Failure(
                    Error.Failure(ErrorCodes.OTP.Expired, "کد تایید منقضی شده است."));
            }

            if (otp.IsLockedOut(
                _otpSettings.MaxRequestsPerWindow, 
                now,
                TimeSpan.FromMinutes(_otpSettings.RateLimitWindowMinutes)))
            {
                return Result<VerifyOtpResponse>.Failure(
                    Error.Failure(ErrorCodes.OTP.TooManyRequests, "حساب شما به مدت ده دقیقه قفل شده است."));
            }

            if (!otp.Code.Equals(request.Code))
            {
                otp.RecordFailedAttempt();
                await _otpCodeRepository.UpdateAsync(otp);

                return Result<VerifyOtpResponse>.Failure(
                    Error.Failure(ErrorCodes.OTP.Invalid, "کد تایید اشتباه است."));
            }

            otp.MarkAsUsed(now);
            await _otpCodeRepository.UpdateAsync(otp);

            var user = await _userRepository.GetByPhoneNumberAsync(phoneNumber);

            if (user is null)
            {
                user = new User(phoneNumber);

                await _userRepository.AddAsync(user);
            }

            user.VerifyPhone();

            var accessToken = _tokenService.GenerateAccessToken(user);

            var (rawRefreshToken, refreshTokenEntity) =
                _refreshTokenDomainService.Generate(user, now, expiry);

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
