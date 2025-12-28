using Microsoft.EntityFrameworkCore;
using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Interfaces;


namespace PosterrBackend.Infrastructure.Repositories
{
	public class HelperRepository : IHelperRepository
	{
        private readonly AppDbContext _context;

        public HelperRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Record>> GetSortedPostsByPage(int page)
        {
            int skipAmount, takeAmount;
            (skipAmount, takeAmount) = Helper.GetSkipAndTakeAmount(page);

            var postsQuery = _context.Post
            .Select(p => new Record
            {
                Id = p.Id,
                Text = p.Text,
                Creator = p.Creator,
                CreationDate = p.CreationDate,
                IsPost = true
            });

            var repostsQuery = _context.Repost
                .Join(_context.Post,
                        r => r.IdPost,
                        p => p.Id,
                        (r, p) => new Record
                        {
                            Id = r.Id,
                            Text = p.Text,
                            Creator = r.Creator,
                            CreationDate = r.CreationDate,
                            IsPost = false
                        });

            var postsList = await postsQuery.ToListAsync();
            var repostsList = await repostsQuery.ToListAsync();

            var mergedList = postsList.Concat(repostsList)
                                .OrderByDescending(r => r.CreationDate)
                                .Skip(skipAmount)
                                .Take(takeAmount);

            return mergedList.ToList();
        }

        public async Task<List<Post>> GetSortedPostsByTrend(int page)
        {
            int skipAmount, takeAmount;
            (skipAmount, takeAmount) = Helper.GetSkipAndTakeAmount(page);

            var postsWithReposts = await _context.Post
                .GroupJoin(
                    _context.Repost,
                    post => post.Id,
                    repost => repost.IdPost,
                    (post, reposts) => new
                    {
                        Post = post,
                        RepostCount = reposts.Count()
                    })
                .OrderByDescending(x => x.RepostCount) 
                .ThenByDescending(x => x.Post.CreationDate)
                .Skip(skipAmount)
                .Take(takeAmount)
                .Select(x => x.Post)
                .ToListAsync();

            return postsWithReposts;
        }
    }
}

