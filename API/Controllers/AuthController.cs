using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.Features.Auth.SendOtp;

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
        public async Task<IActionResult> SendOtp(
            [FromBody] SendOtpCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return Ok(result);
        }
    }
}