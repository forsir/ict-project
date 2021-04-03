using System;
using System.Text.Json.Serialization;

namespace Forsir.IctProject.Web.Infrastructure.Models
{
	public class RefreshToken
	{
		[JsonPropertyName("username")]
		public string UserName { get; set; } = String.Empty;

		[JsonPropertyName("tokenString")]
		public string TokenString { get; set; } = String.Empty;

		[JsonPropertyName("expireAt")]
		public DateTime ExpireAt { get; set; }
	}
}
