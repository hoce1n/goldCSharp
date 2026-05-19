using Application.Common.Result;
using Application.Features.Auth.Command.CompleteRegistration;
using Application.Features.Auth.Command.Logout;
using Application.Features.Auth.Command.RefreshToken;
using Application.Features.Auth.Command.SendOtp;
using Application.Features.Auth.Command.VerifyOtp;
using Application.Features.Auth.Query.GetCurrentUser;
using Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(
            ISender sender,
            IHttpContextAccessor httpContextAccessor)
        {
            _sender = sender;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost("send-otp")]
        public async Task<Result<SendOtpResponse>> SendOtp(
            [FromBody] SendOtpCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return result;
        }

        [HttpPost("verify-otp")]
        public async Task<Result<VerifyOtpResponse>> VerfiyOtp(
            [FromBody] VerifyOtpCommand command,
            CancellationToken cancellationToken)

        {
            var result = await _sender.Send(command, cancellationToken);

            if (!string.IsNullOrWhiteSpace(result.Value.RawRefreshToken))
            {
                Response.Cookies.Append("refreshToken", result.Value.RawRefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.UtcNow.AddDays(7)
                });
            }

            return result;
        }

        [Authorize]
        [HttpPost("complete-registration")]
        public async Task<Result<CompleteRegistrationResponse>> CompleteRegistration(
            [FromBody] CompleteRegistrationCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return result;
        }

        [HttpPost("refresh-token")]
        public async Task<Result<RefreshTokenResponse>> RefreshToken(
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

            var result = await _sender.Send(new RefreshTokenCommand(rawRefresh), cancellationToken);

            http.Response.Cookies.Append("refreshToken", result.Value.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return result;
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<Result<LogoutResponse>> Logout(
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

            var result = await _sender.Send(new LogoutCommand(rawRefresh), cancellationToken);

            return result;
        }

        [Authorize]
        [HttpGet("me")]
        public async Task<Result<GetCurrentUserResponse>> Me()
        {
            var result = await _sender.Send(new GetCurrentUserQuery());
            return result;
        }
    }
}