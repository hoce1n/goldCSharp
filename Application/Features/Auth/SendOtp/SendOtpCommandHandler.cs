using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Entities.Identity;
using Domain.ValueObjects;

namespace Application.Features.Auth.SendOtp
{
    public sealed class SendOtpCommandHandler
        : ICommandHandler<SendOtpCommand, Result<SendOtpResponse>>
    {
        private readonly IOtpCodeRepository _otpCodeRepository;
        private readonly ISMSService _smsService;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;

        public SendOtpCommandHandler(
            IOtpCodeRepository otpCodeRepository,
            ISMSService smsService, 
            IDateTimeProvider dateTime,
            IUnitOfWork unitOfWork)
        {
            _otpCodeRepository = otpCodeRepository;
            _smsService = smsService;
            _dateTimeProvider = dateTime;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<SendOtpResponse>> Handle(
            SendOtpCommand request,
            CancellationToken cancellationToken)
        {
            var phone = new PhoneNumber(request.PhoneNumber);

            var existing = await _otpCodeRepository
                .GetActiveCodeAsync(phone, cancellationToken);

            if (existing is not null && !existing.IsExpired())
            {
                return Result<SendOtpResponse>.Failure(
                    Error.Conflict("An OTP was already sent. Please wait.")
                );
            }

            var code = GenerateOtp();

            var otp = new OtpCode(
                phone,
                code,
                _dateTimeProvider.UtcNow.AddMinutes(2)
            );

            await _otpCodeRepository.AddAsync(otp);
            await _unitOfWork.SaveChangeAsync(cancellationToken);

            await _smsService.SendOtpAsync(
                phone,
                $"Your OTP code is: {code}",
                cancellationToken);

            var response = new SendOtpResponse(
                request.PhoneNumber,
                120
            );

            return response;
        }

        private static string GenerateOtp()
        {
            var randomBytes = new byte[4];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);

            var value = BitConverter.ToUInt32(randomBytes, 0);
            var otp = (value % 900000) + 100000;
            return otp.ToString("D6");
        }
    }
}
