using System.Threading.Tasks;
using Application.Profiles;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class ProfilesController : BaseApiController
    {
        [HttpGet("{username}")]
        public async Task<IActionResult> GetProfile(string username)
        {
            return HandleResult(await Mediator.Send(new Details.Query { Username = username }));
        }

        [HttpPut("{username}")]
        public async Task<IActionResult> EditProfile(string username, ProfileDto profile)
        {
            profile.Username = username;
            return HandleResult(await Mediator.Send(new Edit.Command { Profile = profile }));
        }

        [HttpGet("{username}/coins")]
        public async Task<IActionResult> GetProfileCoins(string username, string predicate)
        {
            return HandleResult(await Mediator.Send(new ListCoins.Query
            {
                Username = username,
                Predicate = predicate
            }));
        }

        [HttpGet("subscriptions")]
        public async Task<IActionResult> GetSubscriptions()
        {
            return HandleResult(await Mediator.Send(new ListSubscriptions.Query()));
        }
    }
}