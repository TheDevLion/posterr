using PosterrBackend.Application.RequestModels;
using Swashbuckle.AspNetCore.Filters;

namespace PosterrBackend.PosterrAPI.SwaggerExamples
{
    public class CreateNewPostRequestExample : IExamplesProvider<CreateNewPostRequest>
    {
        public CreateNewPostRequest GetExamples()
        {
            return new CreateNewPostRequest("Example post created", "rodrigoczleo");
        }
    }
}