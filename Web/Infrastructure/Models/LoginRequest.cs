using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Forsir.IctProject.Web.Infrastructure.Models
{
	public class LoginRequest
	{
		[Required]
		[JsonPropertyName("username")]
		public string UserName { get; set; } = String.Empty;

		[Required]
		[JsonPropertyName("password")]
		public string Password { get; set; } = String.Empty;
	}
}
