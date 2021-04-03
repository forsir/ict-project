using System;
using System.Text.Json.Serialization;

namespace Forsir.IctProject.Web.Infrastructure.Models
{
	public class JwtAuthResult
	{
		[JsonPropertyName("accessToken")]
		public string AccessToken { get; set; } = String.Empty;

		[JsonPropertyName("refreshToken")]
		public RefreshToken RefreshToken { get; set; } = new RefreshToken();
	}
}
