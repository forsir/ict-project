using System;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.IdentityModel.Tokens;

namespace Forsir.IctProject.Web.Infrastructure
{
	// Used from https://github.com/dotnet-labs/JwtAuthDemo

	public class JwtAuthManager : IJwtAuthManager
	{
		public IImmutableDictionary<string, RefreshToken> UsersRefreshTokensReadOnlyDictionary => _usersRefreshTokens.ToImmutableDictionary();
		private readonly ConcurrentDictionary<string, RefreshToken> _usersRefreshTokens;  // can store in a database or a distributed cache
		private readonly JwtTokenConfig _jwtTokenConfig;
		private readonly byte[] _secret;

		public JwtAuthManager(JwtTokenConfig jwtTokenConfig)
		{
			_jwtTokenConfig = jwtTokenConfig;
			_usersRefreshTokens = new ConcurrentDictionary<string, RefreshToken>();
			_secret = Encoding.ASCII.GetBytes(jwtTokenConfig.Secret);
		}

		// optional: clean up expired refresh tokens
		public void RemoveExpiredRefreshTokens(DateTime now)
		{
			var expiredTokens = _usersRefreshTokens.Where(x => x.Value.ExpireAt < now).ToList();
			foreach (System.Collections.Generic.KeyValuePair<string, RefreshToken> expiredToken in expiredTokens)
			{
				_usersRefreshTokens.TryRemove(expiredToken.Key, out _);
			}
		}

		// can be more specific to ip, user agent, device name, etc.
		public void RemoveRefreshTokenByUserName(string userName)
		{
			var refreshTokens = _usersRefreshTokens.Where(x => x.Value.UserName == userName).ToList();
			foreach (System.Collections.Generic.KeyValuePair<string, RefreshToken> refreshToken in refreshTokens)
			{
				_usersRefreshTokens.TryRemove(refreshToken.Key, out _);
			}
		}

		public JwtAuthResult GenerateTokens(string username, Claim[] claims, DateTime now)
		{
			bool shouldAddAudienceClaim = String.IsNullOrWhiteSpace(claims?.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Aud)?.Value);
			var jwtToken = new JwtSecurityToken(
				_jwtTokenConfig.Issuer,
				shouldAddAudienceClaim ? _jwtTokenConfig.Audience : String.Empty,
				claims,
				expires: now.AddMinutes(_jwtTokenConfig.AccessTokenExpiration),
				signingCredentials: new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature));
			string accessToken = new JwtSecurityTokenHandler().WriteToken(jwtToken);

			var refreshToken = new RefreshToken
			{
				UserName = username,
				TokenString = GenerateRefreshTokenString(),
				ExpireAt = now.AddMinutes(_jwtTokenConfig.RefreshTokenExpiration)
			};
			_usersRefreshTokens.AddOrUpdate(refreshToken.TokenString, refreshToken, (_, _) => refreshToken);

			return new JwtAuthResult
			{
				AccessToken = accessToken,
				RefreshToken = refreshToken
			};
		}

		public JwtAuthResult Refresh(string refreshToken, string accessToken, DateTime now)
		{
			(ClaimsPrincipal principal, JwtSecurityToken jwtToken) = DecodeJwtToken(accessToken);
			if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature))
			{
				throw new SecurityTokenException("Invalid token");
			}

			string userName = principal.Identity?.Name;
			if (!_usersRefreshTokens.TryGetValue(refreshToken, out RefreshToken existingRefreshToken))
			{
				throw new SecurityTokenException("Invalid token");
			}
			if (existingRefreshToken.UserName != userName || existingRefreshToken.ExpireAt < now)
			{
				throw new SecurityTokenException("Invalid token");
			}

			return GenerateTokens(userName, principal.Claims.ToArray(), now); // need to recover the original claims
		}

		public (ClaimsPrincipal, JwtSecurityToken) DecodeJwtToken(string token)
		{
			if (String.IsNullOrWhiteSpace(token))
			{
				throw new SecurityTokenException("Invalid token");
			}
			ClaimsPrincipal principal = new JwtSecurityTokenHandler()
				.ValidateToken(token,
					new TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidIssuer = _jwtTokenConfig.Issuer,
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(_secret),
						ValidAudience = _jwtTokenConfig.Audience,
						ValidateAudience = true,
						ValidateLifetime = true,
						ClockSkew = TimeSpan.FromMinutes(1)
					},
					out SecurityToken validatedToken);
			return (principal, validatedToken as JwtSecurityToken);
		}

		private static string GenerateRefreshTokenString()
		{
			byte[] randomNumber = new byte[32];
			using var randomNumberGenerator = RandomNumberGenerator.Create();
			randomNumberGenerator.GetBytes(randomNumber);
			return Convert.ToBase64String(randomNumber);
		}
	}

	public class JwtAuthResult
	{
		[JsonPropertyName("accessToken")]
		public string AccessToken { get; set; }

		[JsonPropertyName("refreshToken")]
		public RefreshToken RefreshToken { get; set; }
	}

	public class RefreshToken
	{
		[JsonPropertyName("username")]
		public string UserName { get; set; }    // can be used for usage tracking
												// can optionally include other metadata, such as user agent, ip address, device name, and so on

		[JsonPropertyName("tokenString")]
		public string TokenString { get; set; }

		[JsonPropertyName("expireAt")]
		public DateTime ExpireAt { get; set; }
	}
}
