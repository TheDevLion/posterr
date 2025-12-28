using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Application.Interfaces;
using PosterrBackend.Application.Mapping;
using PosterrBackend.Application.Services;
using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Exceptions;
using PosterrBackend.Domain.Interfaces;
using PosterrBackend.Infrastructure;
using PosterrBackend.Infrastructure.Repositories;

namespace PosterrAPITests;

public class PostTest
{
    private readonly IPostService _postService;

    private AppDbContext CreateInMemoryContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        return new AppDbContext(options);
    }

    private async Task<AppDbContext> CreateTableWithData()
    {
        var dbName = Guid.NewGuid().ToString();
        var context = CreateInMemoryContext(dbName);

        context.Post.Add(new Post
        {
            Id = 1,
            Text = "Test post",
            Creator = "rodrigoczleo",
            CreationDate = DateTime.UtcNow
        });
        context.Post.Add(new Post
        {
            Id = 2,
            Text = "My dream is to be world champion",
            Creator = "neymarjr",
            CreationDate = DateTime.UtcNow
        });
        context.Post.Add(new Post
        {
            Id = 3,
            Text = "Test post 3 - Champion",
            Creator = "johntextor",
            CreationDate = DateTime.UtcNow
        });
        context.Post.Add(new Post
        {
            Id = 4,
            Text = "Test post 4 - champion",
            Creator = "rodrigoczleo",
            CreationDate = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
        return context;
    }

    public PostTest()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("Posterr_Post"));

        serviceCollection.AddScoped<IPostService, PostService>();
        serviceCollection.AddScoped<IPostRepository, PostRepository>();
        serviceCollection.AddScoped<IRepostService, RepostService>();
        serviceCollection.AddScoped<IRepostRepository, RepostRepository>();
        serviceCollection.AddScoped<IHelperService, HelperService>();
        serviceCollection.AddScoped<IHelperRepository, HelperRepository>();

        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddAutoMapper(typeof(MappingProfile).Assembly);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        _postService = serviceProvider.GetRequiredService<IPostService>();
    }

    [Fact]
    public async Task Should_Get_Posts()
    {
        var context = await CreateTableWithData();
        var postRepository = new PostRepository(context);
        var posts = await postRepository.GetPosts();

        Assert.NotNull(posts);
        Assert.Equal(4, posts.Count);

        context.Dispose();
    }

    [Fact]
    public async Task Should_Create_Post()
    {
        var context = await CreateTableWithData();
        var postRepository = new PostRepository(context);

        var newPost = new Post() { Id = 555, Creator = "neymarjr", CreationDate = DateTime.UtcNow, Text = "test" };

        var repostId = await postRepository.CreatePost(newPost);

        Assert.Equal(555, repostId);

        context.Dispose();
    }

    [Fact]
    public async Task Should_Get_Post_By_Id()
    {
        var context = await CreateTableWithData();
        var postRepository = new PostRepository(context);
        var post = await postRepository.GetPostById(2);

        Assert.NotNull(post);
        Assert.Equal("My dream is to be world champion", post.Text);

        context.Dispose();
    }

    [Fact]
    public async Task Should_Get_Daily_User_Posts_In_Day()
    {
        var context = await CreateTableWithData();
        var postRepository = new PostRepository(context);
        var count = await postRepository.CountPostsByUserAndDate("rodrigoczleo", DateTime.UtcNow);

        Assert.Equal(2, count);

        context.Dispose();
    }

    [Fact]
    public async Task Should_Filter_Posts_By_Keyword()
    {
        var context = await CreateTableWithData();
        var postRepository = new PostRepository(context);
        var posts = await postRepository.FilterPostsByKeywords("champion", 0);

        Assert.NotNull(posts);
        Assert.Equal(2, posts.Count);

        context.Dispose();
    }

    [Fact]
    public async Task Should_Create_New_Post_On_Service()
    {
        var temp1 = new PostDTO() { Id = 101, Creator = "rodrigoczleo", Text = "Just one more", CreationDate = DateTime.UtcNow };
        var newPostId = await _postService.CreatePost(temp1);
        Assert.Equal(101, newPostId);

        var temp2 = new PostDTO() { Id = 102, Creator = "rodrigoczleo", Text = "", CreationDate = DateTime.UtcNow };
        var exception = await Assert.ThrowsAsync<NotProperContentException>(() => _postService.CreatePost(temp2));
        Assert.IsType<NotProperContentException>(exception);

        var temp3 = new PostDTO() { Id = 103, Creator = "rodrigoczleo", Text = "to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text, to long text",
                CreationDate = DateTime.UtcNow };
        var exception2 = await Assert.ThrowsAsync<NotProperContentException>(() => _postService.CreatePost(temp3));
        Assert.IsType<NotProperContentException>(exception2);

        for(int i=0; i<4; i++)
        {
            var temp = new PostDTO() { Id = 100 + 4 + i, Creator = "rodrigoczleo", Text = "Just one more", CreationDate = DateTime.UtcNow };
            var newPostIdTemp = await _postService.CreatePost(temp);
        }

        var temp4 = new PostDTO() { Id = 110, Creator = "rodrigoczleo", Text = "another one", CreationDate = DateTime.UtcNow };
        var exception3 = await Assert.ThrowsAsync<DailyActionsExceededException>(() => _postService.CreatePost(temp4));
        Assert.IsType<DailyActionsExceededException>(exception3);
    }

    [Fact]
    public async Task Should_Filter_Posts_By_Keyword_On_Service()
    {
        for (int i = 0; i < 4; i++)
        {
            var temp = new PostDTO() { Id = 120 + 4 + i, Creator = "neymarjr", Text = "Just one more", CreationDate = DateTime.UtcNow };
            var newPostIdTemp = await _postService.CreatePost(temp);
        }
        var posts = await _postService.FilterPostsByKeywords("one", 0);
        Assert.NotNull(posts);
        Assert.Equal(4, posts.Count);
    }
}
