using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PosterrBackend.Application.DTOs;
using PosterrBackend.Application.Interfaces;
using PosterrBackend.Application.Mapping;
using PosterrBackend.Application.Services;
using PosterrBackend.Domain.Entities;
using PosterrBackend.Domain.Enums;
using PosterrBackend.Domain.Interfaces;
using PosterrBackend.Infrastructure;
using PosterrBackend.Infrastructure.Repositories;

namespace PosterrAPITests;

public class HelperTest
{
    private readonly IHelperService _helperService;
    private readonly IPostService _postService;
    private readonly IRepostService _repostService;

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
            CreationDate = new DateTime(2025, 1, 9)
        });
        context.Post.Add(new Post
        {
            Id = 2,
            Text = "My dream is to be world champion",
            Creator = "neymarjr",
            CreationDate = new DateTime(2025, 1, 8)
        });
        context.Post.Add(new Post
        {
            Id = 3,
            Text = "Test post 3 - Champion",
            Creator = "johntextor",
            CreationDate = new DateTime(2025, 1, 10)
        });
        context.Post.Add(new Post
        {
            Id = 4,
            Text = "Test post 4 - champion",
            Creator = "rodrigoczleo",
            CreationDate = new DateTime(2025, 1, 8)
        });
        context.Post.Add(new Post
        {
            Id = 5,
            Text = "Test post 5 - champion",
            Creator = "rodrigoczleo",
            CreationDate = new DateTime(2025, 1, 8)
        });
        context.Repost.Add(new Repost
        {
            Id = 1,
            IdPost = 2,
            Creator = "rodrigoczleo",
            CreationDate = new DateTime(2025, 1, 6)
        });
        context.Repost.Add(new Repost
        {
            Id = 2,
            IdPost = 2,
            Creator = "gabrielamatias",
            CreationDate = new DateTime(2025, 1, 8)
        });
        context.Repost.Add(new Repost
        {
            Id = 3,
            IdPost = 3,
            Creator = "rodrigoczleo",
            CreationDate = new DateTime(2025, 1, 8)
        });
        

        await context.SaveChangesAsync();
        return context;
    }

    public HelperTest()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddDbContext<AppDbContext>(options =>
            options.UseInMemoryDatabase("Posterr_Helper"));

        serviceCollection.AddScoped<IPostService, PostService>();
        serviceCollection.AddScoped<IPostRepository, PostRepository>();
        serviceCollection.AddScoped<IRepostService, RepostService>();
        serviceCollection.AddScoped<IRepostRepository, RepostRepository>();
        serviceCollection.AddScoped<IHelperService, HelperService>();
        serviceCollection.AddScoped<IHelperRepository, HelperRepository>();

        serviceCollection.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        serviceCollection.AddAutoMapper(typeof(MappingProfile).Assembly);

        var serviceProvider = serviceCollection.BuildServiceProvider();

        _helperService = serviceProvider.GetRequiredService<IHelperService>();
        _postService = serviceProvider.GetRequiredService<IPostService>();
        _repostService = serviceProvider.GetRequiredService<IRepostService>();
    }

    [Fact]
    public async Task Should_Get_Sorted_Records_By_Date_Desc()
    {
        var context = await CreateTableWithData();
        var helperRepository = new HelperRepository(context);
        var records = await helperRepository.GetSortedPostsByPage(0);
        
        Assert.NotNull(records);
        Assert.Equal(8, records.Count);
        Assert.Equal("Test post", records[1].Text);

        context.Dispose();
    }

    [Fact]
    public async Task Should_Get_Sorted_Records_By_Trend()
    {
        var context = await CreateTableWithData();
        var helperRepository = new HelperRepository(context);
        var records = await helperRepository.GetSortedPostsByTrend(0);

        Assert.NotNull(records);
        Assert.Equal(5, records.Count);
        Assert.Equal("My dream is to be world champion", records[0].Text);

        context.Dispose();
    }

    [Fact]
    public async Task Has_User_Exceed_Daily_Actions_Limit()
    {
        for (int i=0; i<5; i++)
        {
            var temp = new PostDTO() { Id = 400 + i, Creator = "usertest1", Text = $"Test message ({i})", CreationDate = DateTime.UtcNow };
            var newPostIdTemp = await _postService.CreatePost(temp);
        }

        for (int i = 0; i < 2; i++)
        {
            var temp = new RepostDTO() { Id = 500 + i, Creator = "usertest2", IdPost = 400 + i, CreationDate = DateTime.UtcNow };
            var newPostIdTemp = await _repostService.CreateRepost(temp);
        }

        var result1 = await _helperService.HasUserExceededDailyActions("usertest1");
        var result2 = await _helperService.HasUserExceededDailyActions("usertest2");

        Assert.True(result1);
        Assert.False(result2);
    }

    [Fact]
    public async Task Should_Post_Be_Available_For_Repost()
    {
        for(int i = 0; i < 5; i++)
        {
            var temp = new PostDTO() { Id = 600 + i, Creator = "usertest3", Text = $"Test message ({i})", CreationDate = DateTime.UtcNow };
            var newPostIdTemp = await _postService.CreatePost(temp);
        }

        var temp2 = new PostDTO() { Id = 605, Creator = "usertest4", Text = $"Test message (5)", CreationDate = DateTime.UtcNow };
        var newPostIdTemp2 = await _postService.CreatePost(temp2);

        for (int i = 0; i < 2; i++)
        {
            var temp = new RepostDTO() { Id = 700 + i, Creator = "usertest4", IdPost = 600 + i, CreationDate = DateTime.UtcNow };
            var newPostIdTemp = await _repostService.CreateRepost(temp);
        }

        var result1 = await _helperService.IsRepostAvailableForUser("usertest3", 605);
        var result2 = await _helperService.IsRepostAvailableForUser("usertest4", 602);
        var result3 = await _helperService.IsRepostAvailableForUser("usertest4", 605);
        var result4 = await _helperService.IsRepostAvailableForUser("usertest4", 601);
        var result5 = await _helperService.IsRepostAvailableForUser("usertest4", 5001);

        Assert.Equal(RepostAvailabilityStatus.UserExceededDailyActions, result1);
        Assert.Equal(RepostAvailabilityStatus.AvailableToRepost, result2);
        Assert.Equal(RepostAvailabilityStatus.UserIsOwner, result3);
        Assert.Equal(RepostAvailabilityStatus.AlreadyRepostedByUser, result4);
        Assert.Equal(RepostAvailabilityStatus.PostNonExistent, result5);
    }

    [Fact]
    public async Task Should_Load_Records()
    {
        for (int i = 0; i < 5; i++)
        {
            var temp = new PostDTO() { Id = 800 + i, Creator = "usertest5", Text = $"Test message ({i})", CreationDate = new DateTime(2025, 1, i + 1) };
            var newPostIdTemp = await _postService.CreatePost(temp);
        }

        var temp2 = new RepostDTO() { Id = 901, Creator = "usertest6", IdPost = 800, CreationDate = new DateTime(2025, 1, 6) };
        var newPostIdTemp2 = await _repostService.CreateRepost(temp2);

        var temp3 = new RepostDTO() { Id = 902, Creator = "usertest7", IdPost = 800, CreationDate = new DateTime(2025, 1, 7) };
        var newPostIdTemp3 = await _repostService.CreateRepost(temp3);
        

        var result1 = await _helperService.LoadRecords(0);
        var result2 = await _helperService.LoadRecords(0, false);

        Assert.Equal(902, result1[0].Id);
        Assert.Equal(800, result2[0].Id);
    }

}
