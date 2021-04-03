using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Forsir.IctProject.Web.Infrastructure.Models;
using Microsoft.IdentityModel.Tokens;

namespace Forsir.IctProject.Web.Infrastructure
{
	public interface IJwtAuthManager
	{
		IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary { get; }

		JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now);

		JwtAuthResult Refresh(string refreshToken, string? accessToken, DateTime now);

		void RemoveExpiredRefreshTokens(DateTime now);

		void RemoveRefreshTokenByUserName(string userName);

		(ClaimsPrincipal, JwtSecurityToken?) DecodeJwtToken(string token);
	}
}
