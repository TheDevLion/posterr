namespace PosterrBackend.Application.RequestModels
{
	public class CreateNewRepostRequest
	{
        public int IdPost { get; set; }
        public string? Creator { get; set; }

        public CreateNewRepostRequest(int idPost, string creator)
        {
            IdPost = idPost;
            Creator = creator;
        }
	}
}

