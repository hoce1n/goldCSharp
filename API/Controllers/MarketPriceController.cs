using Application.Common.Result;
using Application.Features.Catalog.MarketPrices.Queries.GetLatest;
using Application.Features.Catalog.MarketPrices.Queries.History;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MarketPriceController : ControllerBase
    {
        private readonly ISender _sender;

        public MarketPriceController(ISender sender)
        {
            _sender = sender;
        }

        [HttpGet("latest")]
        [AllowAnonymous]
        public async Task<Result<GetLatestMarketPriceResponse>> GetLatest()
        {
            var result = await _sender.Send(new GetLatestMarketPriceQuery());
            return result;
        }

        [HttpGet("history")]
        [AllowAnonymous]
        public async Task<Result<GetMarketPriceHistoryResponse>> GetHistory(
            [FromQuery] MarketPriceHistoryRange range = MarketPriceHistoryRange.Day)
        {
            return await _sender.Send(new GetMarketPriceHistoryQuery(range));
        }

    }
}
