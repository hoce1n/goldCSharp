using Application.Abstractions.Configuration;
using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Abstractions.Security;
using Domain.Common.Errors;
using Domain.Services.Identity;

namespace Application.Features.Auth.Command.RefreshToken
{
    public class RefreshTokenCommandHandler
        : ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly ITokenHasher _tokenHasher;
        private readonly ITokenService _tokenService;
        private readonly RefreshTokenDomainService _refreshTokenDomainService;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthSetting _authSetting;

        public RefreshTokenCommandHandler(
            IDateTimeProvider dateTimeProvider,
            ITokenHasher tokenHasher,
            ITokenService tokenService,
            RefreshTokenDomainService refreshTokenDomainService,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IAuthSetting authSetting,
            IUnitOfWork unitOfWork)
        {
            _dateTimeProvider = dateTimeProvider;
            _tokenHasher = tokenHasher;
            _tokenService = tokenService;
            _refreshTokenDomainService = refreshTokenDomainService;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _authSetting = authSetting;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(
            RefreshTokenCommand command,
            CancellationToken cancellationToken)
         {
            var now = _dateTimeProvider.UtcNow;
            var hash = _tokenHasher.Hash(command.RefreshToken);

            var refreshToken = await _refreshTokenRepository
                .GetByHashAsync(hash, cancellationToken);

            if (refreshToken is null)
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Invalid, "رفرش توکن، وجود ندارد."));


            var user = await _userRepository
                .GetByIdWithRefreshTokensAsync(refreshToken.UserId, cancellationToken);

            if (user is null)
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.User.NotFound, "این رفرش توکن، متعلق به کاربری نیست."));

            if (refreshToken.IsExpired(now))
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Expired, "رفرش توکن، منقضی شده است."));

            if (!refreshToken.IsActive(now))
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Revoked, "رفرش توکن، فعال نیست."));

            _refreshTokenDomainService.EnforceReplayDefense(
                user, 
                command.RefreshToken,
                now);

            await _userRepository.UpdateAsync(user, cancellationToken);

            var expiry = TimeSpan.FromDays(_authSetting.RefreshTokenExpiryDays);

            var (rawNew, newTokenEntity) =
                _refreshTokenDomainService.Rotate(
                    user, 
                    refreshToken, 
                    now,
                    expiry);

            //await _refreshTokenRepository.UpdateAsync(refreshToken);
            await _refreshTokenRepository.AddAsync(newTokenEntity);

            _refreshTokenDomainService.EnforceTokenLimit(
                user, 
                _authSetting.MaxActiveRefreshTokens,
                now);

            var newAccessToken = _tokenService.GenerateAccessToken(user);

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var response = new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = rawNew,
                UserId = user.Id,
                PhoneNumber = user.PhoneNumber.Value,
                VerificationLevel = (int)user.VerificationLevel,
                Status = user.Status.ToString(),
                IsProfileCompleted = user.IsProfileCompleted
            };

            return response;
        }
    }
}