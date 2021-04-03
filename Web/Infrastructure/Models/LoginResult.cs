using System;
using System.Text.Json.Serialization;

namespace Forsir.IctProject.Web.Infrastructure.Models
{
	public class LoginResult
	{
		[JsonPropertyName("username")]
		public string UserName { get; set; } = String.Empty;

		[JsonPropertyName("originalUserName")]
		public string OriginalUserName { get; set; } = String.Empty;

		[JsonPropertyName("accessToken")]
		public string AccessToken { get; set; } = String.Empty;

		[JsonPropertyName("refreshToken")]
		public string RefreshToken { get; set; } = String.Empty;
	}
}
