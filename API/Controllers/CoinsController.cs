using Application.Common.Result;
using Application.Features.Catalog.Coins.Commands.CreateCoin;
using Application.Features.Catalog.Coins.Queries.GetAllCoins;
using Application.Features.Catalog.Coins.Queries.GetCoinById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly ISender _sender;

        public CoinsController(ISender sender)
        {
            _sender = sender;
        }
        
        //[Authorize(Policy = "AdminOnly")]
        [HttpPost]
        public async Task<Result<CreateCoinResponse>> CreateCoin(
            [FromBody] CreateCoinCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _sender.Send(command, cancellationToken);
            return result;
        }


        [HttpGet]
        public async Task<Result<GetAllCoinsResponse>> GetAll(
            [FromQuery] bool? isActive = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = new GetAllCoinsQuery(isActive, pageNumber, pageSize);
            return await _sender.Send(query);
        }

        [HttpGet("{id:guid}")]
        [AllowAnonymous]
        public async Task<Result<GetCoinByIdResponse>> GetById(Guid id)
        {
            var result = await _sender.Send(new GetCoinByIdQuery(id));
            return result;
        }
    }
}
