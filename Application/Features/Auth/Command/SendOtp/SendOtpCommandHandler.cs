using Application.Abstractions.Configuration;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Common.Errors;
using Domain.Entities.Identity;
using Domain.ValueObjects;

namespace Application.Features.Auth.Command.SendOtp
{
    public sealed class SendOtpCommandHandler
        : ICommandHandler<SendOtpCommand, Result<SendOtpResponse>>
    {
        private readonly IOtpCodeRepository _otpCodeRepository;
        private readonly ISMSService _smsService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IOtpSettings _otpSettings;

        public SendOtpCommandHandler(
            IOtpCodeRepository otpCodeRepository,
            ISMSService smsService, 
            IDateTimeProvider dateTime,
            IUnitOfWork unitOfWork,
            IOtpSettings otpSettings)
        {
            _otpCodeRepository = otpCodeRepository;
            _smsService = smsService;
            _dateTimeProvider = dateTime;
            _unitOfWork = unitOfWork;
            _otpSettings = otpSettings;
        }

        public async Task<Result<SendOtpResponse>> Handle(
            SendOtpCommand request,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;
            var phone = new PhoneNumber(request.PhoneNumber);

            var rateLimitWindow = TimeSpan.FromMinutes(_otpSettings.RateLimitWindowMinutes);

            var activeOtps = await _otpCodeRepository.GetActiveCodesAsync(phone,
                cancellationToken);

            if (!OtpCode.CanRequestNewOtp(
                activeOtps, 
                now,
                rateLimitWindow,
                _otpSettings.MaxRequestsPerWindow))

                return Result<SendOtpResponse>.Failure(
                    Error.Failure(ErrorCodes.OTP.TooManyRequests, "کد قبلی که ارسال شده هنوز فعال است."));

            var code = GenerateOtp();

            var otp = OtpCode.Create(
                phone,
                code,
                now.AddMinutes(_otpSettings.ExpiryMinutes)
            );

            await _otpCodeRepository.AddAsync(otp);

            await _smsService.SendOtpAsync(
                phone,
                $"Your OTP code is: {code}",
                cancellationToken);

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var response = new SendOtpResponse(
                request.PhoneNumber,
                _otpSettings.ExpiryMinutes
            );

            return response;
        }

        private static string GenerateOtp()
        {
            var randomBytes = new byte[4];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            var value = BitConverter.ToUInt32(randomBytes, 0);
            var otp = value % 900000 + 100000;
            return otp.ToString("D6");
        }
    }
}
