using PosterrBackend.Application.DTOs;

namespace PosterrBackend.Application.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDTO>> GetUsers();
        Task<bool> IsUserValid(string? userName);
    }
}
