using Microsoft.EntityFrameworkCore;
using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Enums;
using PosterrBackend.Domain.Interfaces;

namespace PosterrBackend.Infrastructure.Repositories
{
    public class RepostRepository : IRepostRepository
    {
        private readonly AppDbContext _context;

        public RepostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Repost>> GetReposts()
        {
            return await _context.Repost.ToListAsync();
        }

        public async Task<int> CreateRepost(Repost newRepost)
        {
            await _context.Repost.AddAsync(newRepost);
            await _context.SaveChangesAsync();
            return newRepost.Id;
        }


        public async Task<bool> HasUserAlreadyRepostedThisPost(string userName, int idPost)
        {
            return await _context.Repost.AnyAsync(r => r.IdPost == idPost && r.Creator == userName);
        }

        public async Task<int> CountRepostsByUserAndDate(string userName, DateTime date)
        {
            return await _context.Repost
                .Where(r => r.Creator == userName && r.CreationDate.Date == date.Date)
                .CountAsync();
        }
    }
}
