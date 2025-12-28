using Microsoft.EntityFrameworkCore;
using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Interfaces;

namespace PosterrBackend.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        public async Task<bool> IsUserValid(string userName)
        {
            var exists = await _context.User
                .AnyAsync(u => u.UserName == userName);

            return exists;
        }

    }
}
