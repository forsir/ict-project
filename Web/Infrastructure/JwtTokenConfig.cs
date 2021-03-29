using System.Text.Json.Serialization;

namespace Forsir.IctProject.Web.Infrastructure
{
	// Used from https://github.com/dotnet-labs/JwtAuthDemo

	public class JwtTokenConfig
	{
		[JsonPropertyName("secret")]
		public string Secret { get; set; }

		[JsonPropertyName("issuer")]
		public string Issuer { get; set; }

		[JsonPropertyName("audience")]
		public string Audience { get; set; }

		[JsonPropertyName("accessTokenExpiration")]
		public int AccessTokenExpiration { get; set; }

		[JsonPropertyName("refreshTokenExpiration")]
		public int RefreshTokenExpiration { get; set; }
	}
}
