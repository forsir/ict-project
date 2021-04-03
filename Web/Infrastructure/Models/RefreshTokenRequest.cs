using System;
using System.Text.Json.Serialization;

namespace Forsir.IctProject.Web.Infrastructure.Models
{
	public class RefreshTokenRequest
	{
		[JsonPropertyName("refreshToken")]
		public string RefreshToken { get; set; } = String.Empty;
	}
}
