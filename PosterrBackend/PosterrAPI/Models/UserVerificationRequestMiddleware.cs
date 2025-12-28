using System.Text.Json.Serialization;

namespace PosterrAPI.Models
{
	public class UserVerificationRequestMiddleware
	{
        [JsonPropertyName("text")]
        public string? Text { get; set; }
        [JsonPropertyName("creator")]
        public string? Creator { get; set; }

        public UserVerificationRequestMiddleware(string text, string creator)
        {
            Text = text;
            Creator = creator;
        }
            
	}
}

