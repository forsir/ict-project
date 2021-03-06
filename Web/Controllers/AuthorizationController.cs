using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Forsir.IctProject.BusinessLayer.Services;
using Forsir.IctProject.Web.Infrastructure;
using Forsir.IctProject.Web.Infrastructure.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Forsir.IctProject.Web.Controllers
{
	[ApiController]
	[Authorize]
	[Route("api/[controller]")]
	public class AccountController : ControllerBase
	{
		private readonly ILogger<AccountController> _logger;
		private readonly IUserService _userService;
		private readonly IJwtAuthManager _jwtAuthManager;

		public AccountController(ILogger<AccountController> logger, IUserService userService, IJwtAuthManager jwtAuthManager)
		{
			_logger = logger;
			_userService = userService;
			_jwtAuthManager = jwtAuthManager;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public async Task<ActionResult> LoginAsync([FromBody] LoginRequest request)
		{
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			if (!(await _userService.IsValidUserCredentialsAsync(request.UserName, request.Password)))
			{
				return Unauthorized();
			}

			JwtAuthResult jwtResult = _jwtAuthManager.GenerateTokens(request.UserName, new Claim[] { }, DateTime.Now);
			_logger.LogInformation($"User [{request.UserName}] logged in the system.");
			return Ok(new LoginResult
			{
				UserName = request.UserName,
				AccessToken = jwtResult.AccessToken,
				RefreshToken = jwtResult.RefreshToken.TokenString
			});
		}

		[HttpPost("logout")]
		[Authorize]
		public ActionResult Logout()
		{
			// optionally "revoke" JWT token on the server side --> add the current token to a block-list
			// https://github.com/auth0/node-jsonwebtoken/issues/375

			string userName = User.Identity?.Name ?? String.Empty;
			_jwtAuthManager.RemoveRefreshTokenByUserName(userName);
			_logger.LogInformation($"User [{userName}] logged out the system.");
			return Ok();
		}

		[HttpPost("refresh-token")]
		[Authorize]
		public async Task<ActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
		{
			try
			{
				string userName = User.Identity?.Name ?? String.Empty;
				_logger.LogInformation($"User [{userName}] is trying to refresh JWT token.");

				if (String.IsNullOrWhiteSpace(request.RefreshToken))
				{
					return Unauthorized();
				}

				string? accessToken = await HttpContext.GetTokenAsync("Bearer", "access_token");
				JwtAuthResult jwtResult = _jwtAuthManager.Refresh(request.RefreshToken, accessToken, DateTime.Now);
				_logger.LogInformation($"User [{userName}] has refreshed JWT token.");
				return Ok(new LoginResult
				{
					UserName = userName,
					AccessToken = jwtResult.AccessToken,
					RefreshToken = jwtResult.RefreshToken.TokenString
				});
			}
			catch (SecurityTokenException e)
			{
				return Unauthorized(e.Message); // return 401 so that the client side can redirect the user to login page
			}
		}
	}
}
