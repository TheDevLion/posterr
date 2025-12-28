namespace PosterrBackend.Domain.Exceptions
{
	public class AlreadyRepostedByUserException : Exception
    {
		public AlreadyRepostedByUserException(string message) : base(message) {}
	}
}

