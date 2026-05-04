using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Common.Interfaces;
using Application.Common.Result;
using Application.Features.Users.Specifications;
using Domain.Abstractions.Security;
using Domain.Common.Errors;
using Domain.Services.Identity;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Command.RefreshToken
{
    public class RefreshTokenCommandHandler
        : ICommandHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenHasher _tokenHasher;
        private readonly ITokenService _tokenService;
        private readonly RefreshTokenDomainService _refreshTokenDomainService;
        private readonly IUserRepository _userRepository;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            IHttpContextAccessor httpContextAccessor,
            ITokenHasher tokenHasher,
            ITokenService tokenService,
            RefreshTokenDomainService refreshTokenDomainService,
            IRefreshTokenRepository refreshTokenRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenHasher = tokenHasher;
            _tokenService = tokenService;
            _refreshTokenDomainService = refreshTokenDomainService;
            _refreshTokenRepository = refreshTokenRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(
            RefreshTokenCommand command,
            CancellationToken cancellationToken)
        {
            var http = _httpContextAccessor.HttpContext;

            if (http is null)
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.General.Unexpected, "Internal server error."));

            var rawRefresh = http.Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(rawRefresh))
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Unauthorized, "Refresh token missing"));
            var hash = _tokenHasher.Hash(rawRefresh);

            var refreshToken = await _refreshTokenRepository
                .GetByHashAsync(hash);

            if (refreshToken is null)
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Invalid, "Invalid refresh token."));


            var user = await _userRepository
                .GetByIdWithRefreshTokensAsync(refreshToken.UserId, cancellationToken);

            if (user is null)
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.User.NotFound, "Invalid refresh token."));

            if (refreshToken.IsExpired)
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Expired, "Expired refresh token."));
            if (!refreshToken.IsActive)
                return Result<RefreshTokenResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Revoked, "Revoked refresh token."));

            _refreshTokenDomainService.EnforceReplayDefense(user, rawRefresh);

            var (rawNew, newTokenEntity) =
                _refreshTokenDomainService.Rotate(user, refreshToken);

            user.ReplaceRefreshToken(refreshToken.Id, newTokenEntity);

            await _refreshTokenRepository.AddAsync(newTokenEntity);
            await _refreshTokenRepository.UpdateAsync(refreshToken);

            _refreshTokenDomainService.EnforceTokenLimit(user);

            await _unitOfWork.SaveChangeAsync(cancellationToken);

            var newAccessToken = _tokenService.GenerateAccessToken(user);

            http.Response.Cookies.Append("refreshToken", rawNew, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = newTokenEntity.ExpiresAt
            });

            var response = new RefreshTokenResponse
            {
                AccessToken = newAccessToken,
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