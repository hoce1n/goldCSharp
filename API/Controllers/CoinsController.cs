using API.Contracts.Coin;
using Application.Common.Result;
using Application.Features.Catalog.Coins.Commands.ActivateCoin;
using Application.Features.Catalog.Coins.Commands.CreateCoin;
using Application.Features.Catalog.Coins.Commands.DeactivateCoin;
using Application.Features.Catalog.Coins.Commands.DeleteCoin;
using Application.Features.Catalog.Coins.Commands.UpdateCoin;
using Application.Features.Catalog.Coins.Commands.UpdateMintingFee;
using Application.Features.Catalog.Coins.Commands.UpdateStock;
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


        [HttpPut("{id:guid}")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<Result<UpdateCoinResponse>> Update(
            Guid id,
            [FromBody] UpdateCoinRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateCoinCommand(
                id,
                request.Name,
                request.WeightInSoot,
                request.Karat,
                request.ImageUrl,
                request.Description
            );

            var result = await _sender.Send(command, cancellationToken);
            return result;
        }


        [HttpPatch("{id:guid}/stock")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<Result<UpdateCoinStockResponse>> UpdateStock(
            Guid id,
            [FromBody] UpdateCoinStockRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateCoinStockCommand(
                id,
                request.Stock
            );

            var result = await _sender.Send(command, cancellationToken);
            return result;
        }


        [HttpPatch("{id:guid}/minting-fee")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<Result<UpdateCoinMintingFeeResponse>> SetMintingFee(
            Guid id,
            [FromBody] UpdateCoinMintingFeeRequest request,
            CancellationToken cancellationToken)
        {
            var command = new UpdateCoinMintingFeeCommand(
                id,
                request.MintingFee
            );

            var result = await _sender.Send(command, cancellationToken);
            return result;
        }

        [HttpPatch("{id:guid}/activate")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<Result<ActivateCoinResponse>> Activate(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new ActivateCoinCommand(id);
            return await _sender.Send(command, cancellationToken);
        }

        [HttpPatch("{id:guid}/deactivate")]
        //[Authorize(Policy = "AdminOnly")]
        public async Task<Result<DeactivateCoinResponse>> Deactivate(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeactivateCoinCommand(id);
            return await _sender.Send(command, cancellationToken);
        }


        [HttpDelete("{id:guid}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<Result> Delete(
            Guid id,
            CancellationToken cancellationToken)
        {
            var command = new DeleteCoinCommand(id);
            return await _sender.Send(command, cancellationToken);
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
