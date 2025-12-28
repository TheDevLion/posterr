namespace PosterrBackend.Domain.Exceptions
{
	public class UserIsOwnerException : Exception
    {
		public UserIsOwnerException(string message) : base(message) {}
	}
}

