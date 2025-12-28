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

public class RepostTest
{
    private readonly IRepostService _repostService;
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
        context.Repost.Add(new Repost
        {
            Id = 1,
            IdPost = 2,
            Creator = "rodrigoczleo",
            CreationDate = DateTime.UtcNow
        });
        context.Repost.Add(new Repost
        {
            Id = 2,
            IdPost = 2,
            Creator = "gabrielamatias",
            CreationDate = DateTime.UtcNow
        });


        await context.SaveChangesAsync();
        return context;
    }

    public RepostTest()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("Posterr_Repost"));

        serviceCollection.AddScoped<IPostService, PostService>();
        serviceCollection.AddScoped<IPostRepository, PostRepository>();
        serviceCollection.AddScoped<IRepostService, RepostService>();
        serviceCollection.AddScoped<IRepostRepository, RepostRepository>();
        serviceCollection.AddScoped<IHelperService, HelperService>();
        serviceCollection.AddScoped<IHelperRepository, HelperRepository>();

        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddAutoMapper(typeof(MappingProfile).Assembly);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        _repostService = serviceProvider.GetRequiredService<IRepostService>();
        _postService = serviceProvider.GetRequiredService<IPostService>();
    }

    [Fact]
    public async Task Should_Get_Reposts()
    {
        var context = await CreateTableWithData();
        var repostRepository = new RepostRepository(context);
        var reposts = await repostRepository.GetReposts();
        
        Assert.NotNull(reposts);
        Assert.Equal(2, reposts.Count);

        context.Dispose();
    }

    [Fact]
    public async Task Should_Create_Repost()
    {
        var context = await CreateTableWithData();
        var repostRepository = new RepostRepository(context);

        var newRepost = new Repost() { Id = 3, Creator = "neymarjr", CreationDate = DateTime.UtcNow, IdPost = 1 };
        
        var repostId = await repostRepository.CreateRepost(newRepost);

        Assert.Equal(3, repostId);

        context.Dispose();
    }

    [Fact]
    public async Task Has_User_Already_Reposted_The_Post()
    {
        var context = await CreateTableWithData();
        var repostRepository = new RepostRepository(context);

        var userAlreadyReposted = await repostRepository.HasUserAlreadyRepostedThisPost("rodrigoczleo", 2);

        Assert.True(userAlreadyReposted);

        context.Dispose();
    }

    [Fact]
    public async Task Is_User_Reposts_In_Day_Correct()
    {
        var context = await CreateTableWithData();
        var repostRepository = new RepostRepository(context);

        var userDailyReposts = await repostRepository.CountRepostsByUserAndDate("rodrigoczleo", DateTime.UtcNow);

        Assert.Equal(1, userDailyReposts);

        context.Dispose();
    }

    [Fact]
    public async Task Should_Create_Repost_On_Service()
    {
        for (int i = 0; i < 5; i++)
        {
            var temp = new PostDTO() { Id = 200 + i, Creator = "rhuanleao", Text = "Just one more", CreationDate = DateTime.UtcNow };
            var newPostIdTemp = await _postService.CreatePost(temp);
        }

        var temp1 = new PostDTO() { Id = 205, Creator = "neymarjr", Text = "Just one more", CreationDate = DateTime.UtcNow };
        var newPostIdTemp1 = await _postService.CreatePost(temp1);

        for (int i = 0; i < 5; i++)
        {
            var temp2 = new RepostDTO() { Id = 300 + i, Creator = "isisleao", IdPost = 200 + i , CreationDate = DateTime.UtcNow };
            var newRepostId = await _repostService.CreateRepost(temp2);
            Assert.Equal(300 + i, newRepostId);
        }

        var temp3 = new RepostDTO() { Id = 310, Creator = "isisleao",  IdPost = 205, CreationDate = DateTime.UtcNow };
        var exception = await Assert.ThrowsAsync<DailyActionsExceededException>(() => _repostService.CreateRepost(temp3));
        Assert.IsType<DailyActionsExceededException>(exception);

        var temp4 = new RepostDTO() { Id = 311, Creator = "neymarjr", IdPost = 205, CreationDate = DateTime.UtcNow };
        var exception2 = await Assert.ThrowsAsync<UserIsOwnerException>(() => _repostService.CreateRepost(temp4));
        Assert.IsType<UserIsOwnerException>(exception2);

        var temp5 = new RepostDTO() { Id = 312, Creator = "luansantana", IdPost = 205, CreationDate = DateTime.UtcNow };
        var newRepostId2 = await _repostService.CreateRepost(temp5);

        var temp6 = new RepostDTO() { Id = 312, Creator = "luansantana", IdPost = 205, CreationDate = DateTime.UtcNow };
        var exception3 = await Assert.ThrowsAsync<AlreadyRepostedByUserException>(() => _repostService.CreateRepost(temp6));
        Assert.IsType<AlreadyRepostedByUserException>(exception3);

        var temp7 = new RepostDTO() { Id = 313, Creator = "gabrielamatias", IdPost = 1003, CreationDate = DateTime.UtcNow };
        var exception4 = await Assert.ThrowsAsync<PostNonExistentException>(() => _repostService.CreateRepost(temp7));
        Assert.IsType<PostNonExistentException>(exception4);
    }
}
