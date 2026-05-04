using Application.Common.Result;
using Application.Features.Auth.Command.CompleteRegistration;
using Application.Features.Auth.Command.Logout;
using Application.Features.Auth.Command.RefreshToken;
using Application.Features.Auth.Command.SendOtp;
using Application.Features.Auth.Command.VerifyOtp;
using Application.Features.Auth.Query.GetCurrentUser;
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

        public AuthController(ISender sender)
        {
            _sender = sender;
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
            var result = await _sender.Send(new RefreshTokenCommand(), cancellationToken);
            return result;
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<Result<LogoutResponse>> Logout(
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(new LogoutCommand(), cancellationToken);
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