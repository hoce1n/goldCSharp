using API.Contracts.Quote;
using Application.Abstractions.Authentication;
using Application.Common.Result;
using Application.Features.Quotes.Commands.ConfirmQuote;
using Application.Features.Quotes.Commands.CreateQuote;
using Domain.Common.Errors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuoteController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ICurrentUser _currentUser;

        public QuoteController(
            ISender sender,
            ICurrentUser currentUser)
        {
            _sender = sender;
            _currentUser = currentUser;
        }

        //[Authorize]
        [HttpPost]
        public async Task<Result<CreateQuoteResponse>> CreateQuote(
            [FromBody] CreateQuoteRequest request,
            CancellationToken cancellationToken)
        {

            if (_currentUser.UserId is null)
                return Result<CreateQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Auth.Unauthorized, "اجازه درسترسی وجود ندارد."));

            var userId = _currentUser.UserId;

            var command = new CreateQuoteCommand(
                request.ProductType,
                request.Amount,
                request.Side,
                userId.Value,
                request.ProductId
            );

            var result = await _sender.Send(command, cancellationToken);
            return result;
        }

        [Authorize]
        [HttpPost("{quoteId:guid}/confirm")]
        public async Task<Result<ConfirmQuoteResponse>> ConfirmQuote(
            Guid quoteId,
            CancellationToken cancellationToken)
        {
            if (_currentUser.UserId is null)
                return Result<ConfirmQuoteResponse>.Failure(
                    Error.Failure(ErrorCodes.Auth.Unauthorized, "اجازه درسترسی وجود ندارد."));

            var command = new ConfirmQuoteCommand(
                quoteId,
                _currentUser.UserId.Value);

            var result = await _sender.Send(command, cancellationToken);
            return result;
        }
    }
}
