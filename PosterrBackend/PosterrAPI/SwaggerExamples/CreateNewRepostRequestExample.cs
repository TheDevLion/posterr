using PosterrBackend.Application.RequestModels;
using Swashbuckle.AspNetCore.Filters;

namespace PosterrBackend.PosterrAPI.SwaggerExamples
{
	public class CreateNewRepostExample
	{
        public class CreateNewRepostRequestExample : IExamplesProvider<CreateNewRepostRequest>
        {
            public CreateNewRepostRequest GetExamples()
            {
                return new CreateNewRepostRequest(1, "neymarjr");
            }
        }
    }
}

