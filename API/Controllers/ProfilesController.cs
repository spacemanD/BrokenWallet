using System.Threading.Tasks;
using Application.Profiles;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
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

        [HttpPut("subscriptions/{id}")]
        public async Task<IActionResult> Subscribe(int id)
        {
            return HandleResult(await Mediator.Send(new AddSubscriptions.Command
            {
                SubscriptionId = id
            }));
        }

        [HttpGet("users")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> GetUsers()
        {
            return HandleResult(await Mediator.Send(new ListUsers.Query()));
        }

        [HttpPut("ban/{username}")]
        [Authorize(Policy = "IsAdmin")]
        public async Task<IActionResult> BanUser(string username)
        {
            return HandleResult(await Mediator.Send(new BanUser.Command
            {
                Username = username
            }));
        }

        [HttpPost("notifications")]
        public async Task<IActionResult> CreateNotification()
        {
            return HandleResult(await Mediator.Send(new AddNotification.Command()));
        }

        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            return HandleResult(await Mediator.Send(new ListNotifications.Query()));
        }
    }
}