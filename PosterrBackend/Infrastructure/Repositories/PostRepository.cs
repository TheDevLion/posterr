using Microsoft.EntityFrameworkCore;
using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Interfaces;

namespace PosterrBackend.Infrastructure.Repositories
{
    public class PostRepository : IPostRepository
    {
        private readonly AppDbContext _context;

        public PostRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Post>> GetPosts()
        {
            return await _context.Post.ToListAsync();
        }

        public async Task<Post?> GetPostById(int idPost)
        {
            return await _context.Post.FirstOrDefaultAsync(p => p.Id == idPost);
        }

        public async Task<int> CreatePost(Post newPost)
        {
            await _context.Post.AddAsync(newPost);
            await _context.SaveChangesAsync();
            return newPost.Id;
        }

        public async Task<int> CountPostsByUserAndDate(string userName, DateTime date)
        {
            return await _context.Post
                .Where(p => p.Creator == userName && p.CreationDate.Date == date.Date)
                .CountAsync();
        }

        public async Task<List<Post>> FilterPostsByKeywords(string keywords, int page)
        {
            int skipAmount, takeAmount;
            (skipAmount, takeAmount) = Helper.GetSkipAndTakeAmount(page);

            return await _context.Post
                        .Where(p => p.Text.Contains(keywords))
                        .OrderByDescending(x => x.CreationDate)
                        .Skip(skipAmount)
                        .Take(takeAmount)
                        .ToListAsync();
        }

    }
}
