using Application.Abstractions.Messaging;
using Application.Abstractions.Repositories;
using Application.Common.Interfaces;
using Application.Common.Result;
using Domain.Abstractions.Security;
using Domain.Common.Errors;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Auth.Command.Logout
{
    public class LogoutCommandHandler 
        : ICommandHandler<LogoutCommand, Result<LogoutResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenHasher _tokenHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly IUnitOfWork _unitOfWork;

        public LogoutCommandHandler(
            IUserRepository userRepository, 
            ITokenHasher tokenHasher,
            IHttpContextAccessor httpContextAccessor,
            IRefreshTokenRepository refreshTokenRepository,
            IUnitOfWork unitOfWork)
            
        {
            _userRepository = userRepository;
            _tokenHasher = tokenHasher;
            _httpContextAccessor = httpContextAccessor;
            _refreshTokenRepository = refreshTokenRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<LogoutResponse>> Handle(
            LogoutCommand request,
            CancellationToken cancellationToken)
        {
            var http = _httpContextAccessor.HttpContext;

            if (http is null)
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.General.Unexpected, "Internal server error."));

            var rawRefresh = http.Request.Cookies["refreshToken"];
            if (string.IsNullOrWhiteSpace(rawRefresh))
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Unauthorized, "Refresh token missing"));
            var hash = _tokenHasher.Hash(rawRefresh);

            var refreshToken = await _refreshTokenRepository
                .GetByHashAsync(hash);

            if (refreshToken is null)
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Invalid, "Invalid refresh token."));


            var user = await _userRepository
                .GetByIdWithRefreshTokensAsync(refreshToken.UserId, cancellationToken);

            if (user is null)
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.User.NotFound, "Invalid refresh token."));

            if (refreshToken.IsExpired)
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Expired, "Expired refresh token."));
            if (!refreshToken.IsActive)
                return Result<LogoutResponse>.Failure(
                    Error.Failure(ErrorCodes.RefreshToken.Revoked, "Revoked refresh token."));

            refreshToken.Revoke();

            await _refreshTokenRepository.UpdateAsync(refreshToken);
            await _unitOfWork.SaveChangeAsync();

            var response = new LogoutResponse
            {
                RevokedAtUtc = DateTime.UtcNow,
                Message = "با موفقیت خارج شدید."
            };

            return response;
        }
    }
}
