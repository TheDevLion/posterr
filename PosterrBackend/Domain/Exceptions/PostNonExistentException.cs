namespace PosterrBackend.Domain.Exceptions
{
	public class PostNonExistentException : Exception
    {
		public PostNonExistentException(string message) : base(message) {}
	}
}

