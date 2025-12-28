using PosterrBackend.Application.Interfaces;
using AutoMapper;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Domain.Interfaces;

namespace PosterrBackend.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<List<UserDTO>> GetUsers()
        {
            var users = await _userRepository.GetUsers();
            var userDTOs = _mapper.Map<List<UserDTO>>(users);
            return userDTOs;
        }

        public async Task<bool> IsUserValid(string? userName)
        {
            if (userName == null) return false;
            return await _userRepository.IsUserValid(userName);
        }
    }
}
