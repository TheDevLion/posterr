using PosterrBackend.Domain.Entities;

namespace PosterrBackend.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
        Task<bool> IsUserValid(string userName);
    }
}
