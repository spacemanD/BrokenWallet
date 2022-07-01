using System;
using System.Threading.Tasks;
using Application.Coins;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CoinsController : BaseApiController
    {
        [HttpGet]
        public async Task<IActionResult> GetCoins([FromQuery] CoinParams coinParams)
        {
            return HandlePagedResult(await Mediator.Send(new List.Query { Params = coinParams }));
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCoin(Guid id)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Id = id }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateCoin(Coin coin)
        {
            return HandleResult(await Mediator.Send(new Create.Command { Coin = coin }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> EditCoin(Guid id, Coin coin)
        {
            coin.Id = id;
            return HandleResult(await Mediator.Send(new Edit.Command { Coin = coin }));
        }

        [Authorize(Policy = "IsAdmin")]
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCoin(Guid id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        [HttpPost("{id:guid}/attend")]
        public async Task<IActionResult> Attend(Guid id)
        {
            return HandleResult(await Mediator.Send(new UpdateFollowing.Command { Id = id }));
        }
    }
}