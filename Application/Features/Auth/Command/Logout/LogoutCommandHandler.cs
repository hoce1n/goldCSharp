using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Abstractions.Security;
using Domain.Common.Errors;

namespace Application.Features.Auth.Command.Logout
{
    public class LogoutCommandHandler 
        : ICommandHandler<LogoutCommand, Result<LogoutResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHasher _tokenHasher;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDateTimeProvider _dateTimeProvider;

        public LogoutCommandHandler(
            IUserRepository userRepository, 
            ITokenHasher tokenHasher,
            IRefreshTokenRepository refreshTokenRepository,
            IDateTimeProvider dateTimeProvider,
            IUnitOfWork unitOfWork)
            
        {
            _userRepository = userRepository;
            _tokenHasher = tokenHasher;
            _refreshTokenRepository = refreshTokenRepository;
            _dateTimeProvider = dateTimeProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<LogoutResponse>> Handle(
            LogoutCommand request,
            CancellationToken cancellationToken)
        {
            var now = _dateTimeProvider.UtcNow;
            var hash = _tokenHasher.Hash(request.RefreshToken);

            var refreshToken = await _refreshTokenRepository
                .GetByHashAsync(hash, cancellationToken);

            if (refreshToken is null)
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Invalid, "رفرش توکن معتبر نیست."));


            var user = await _userRepository
                .GetByIdWithRefreshTokensAsync(refreshToken.UserId, cancellationToken);

            if (user is null)
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.User.NotFound, "کاربر یافت نشد."));

            if (refreshToken.IsExpired(now))
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Expired, "رفرش توکن منقضی شده است."));
            if (!refreshToken.IsActive(now))
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Revoked, "رفرش توکن قبلا غیرفعال شده است."));

            refreshToken.Revoke(now);

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var response = new LogoutResponse
            {
                RevokedAtUtc = now,
                Message = "با موفقیت خارج شدید."
            };

            return response;
        }
    }
}
