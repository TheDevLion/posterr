using Microsoft.Extensions.DependencyInjection;
using PosterrBackend.Application.Mapping;

public class AutoMapperFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; private set; }

    public AutoMapperFixture()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddAutoMapper(typeof(MappingProfile));

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider?.Dispose();
    }
}