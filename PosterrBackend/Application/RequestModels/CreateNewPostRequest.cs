namespace PosterrBackend.Application.RequestModels
{
	public class CreateNewPostRequest
	{
        public string? Text { get; set; }
        public string? Creator { get; set; }

        public CreateNewPostRequest(string text, string creator)
        {
            Text = text;
            Creator = creator;
        }
    }
}

