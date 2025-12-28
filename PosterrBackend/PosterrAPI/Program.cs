using System.Reflection;
using PosterrBackend.Application.Interfaces;
using PosterrBackend.Application.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PosterrBackend.Domain.Interfaces;
using PosterrBackend.Infrastructure;
using PosterrBackend.Infrastructure.Repositories;
using PosterrBackend.PosterrAPI.SwaggerExamples;
using Swashbuckle.AspNetCore.Filters;
using PosterrBackend.Application.Mapping;
using PosterrBackend.Domain.Entities;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Missing connection string. Set ConnectionStrings__DefaultConnection in environment variables.");
}

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString,
        b => b.MigrationsAssembly("PosterrAPI")));


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IPostService, PostService>();
builder.Services.AddScoped<IRepostRepository, RepostRepository>();
builder.Services.AddScoped<IRepostService, RepostService>();
builder.Services.AddScoped<IHelperRepository, HelperRepository>();
builder.Services.AddScoped<IHelperService, HelperService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);



builder.Services.AddControllers();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Posterr API", Version = "v1" });

    c.EnableAnnotations();
    c.ExampleFilters();

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddSwaggerExamplesFromAssemblyOf<CreateNewPostRequestExample>();


builder.Services.AddCors(options =>
{
    var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
        ?? new[] { "http://localhost:5173" };

    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Posterr API v1");
    });
}
else
{
    app.UseHsts();
}


// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseRouting();

app.UseMiddleware<UserVerificationMiddleware>();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();

    if (!dbContext.User.Any())
    {
        dbContext.User.AddRange(
            new User { UserName = "rodrigoczleo" },
            new User { UserName = "gabrielamatias" },
            new User { UserName = "johntextor" },
            new User { UserName = "neymarjr" }
        );
        dbContext.SaveChanges();
    }
}

app.Run();
