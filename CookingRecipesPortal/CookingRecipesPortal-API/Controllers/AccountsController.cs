using AutoMapper;
using CookingRecipesPortal_DAL.DomainModels;
using CookingRecipesPortal_DAL.DTOs;
using CookingRecipesPortal_DAL.Interfaces.Services;
using CookingRecipesPortal_DAL.Models;
using Microsoft.AspNetCore.Mvc;

namespace CookingRecipesPortal_API.Controllers
{
    public class AccountsController : CookingRecipesPortalController
    {
        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        public AccountsController(IAccountService accountService, IMapper mapper)
        {
            this.accountService = accountService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDto registerDto)
        {
            var createdUser = await accountService.RegisterAsync(mapper.Map<User>(registerDto));

            return Ok(mapper.Map<UserDto>(createdUser));
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var jwtToken = await accountService.LoginAsync(loginDto);
            if (!string.IsNullOrEmpty(jwtToken))
            {
                return Ok(new { Token = jwtToken });
            }

            return Unauthorized();
        }

        [HttpGet("test")]
        public IActionResult Test()
        {
            return Ok("Hello!");
        }

        [HttpPost]
        [Route("{followerId}/follow/{followeeId}")]
        public async Task<IActionResult> FollowAccount([FromRoute] Guid followerId, Guid followeeId)
        {
            await accountService.FollowAccountAsync(followerId, followeeId);
            return Ok();
        }

        [HttpDelete]
        [Route("{followerId}/unfollow/{followeeId}")]
        public async Task<IActionResult> UnfollowAccount([FromRoute] Guid followerId, Guid followeeId)
        {
            await accountService.UnfollowAccountAsync(followerId, followeeId);
            return Ok();
        }

        [HttpGet]
        [Route("{userId}/all-accounts")]
        public async Task<IActionResult> ViewAccounts([FromRoute] Guid userId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            var pagedResponse = await accountService.GetAccountsAsync(new PaginationFilter(pageNumber, pageSize), userId);
            return Ok(pagedResponse);
        }

        [HttpGet]
        [Route("{userId}/followees")]
        public async Task<IActionResult> ViewFollowees([FromRoute] Guid userId, [FromQuery] int pageSize, [FromQuery] int pageNumber)
        {
            var pagedResponse = await accountService.GetFolloweesAsync(userId, new PaginationFilter(pageNumber, pageSize));
            return Ok(pagedResponse);
        }

        [HttpGet]
        [Route("{userId}")]
        public async Task<IActionResult> GetAccountInfo([FromRoute] Guid userId)
        {
            var userModel = await accountService.GetAccountInfoAsync(userId);
            return Ok(userModel);
        }
    }
}
