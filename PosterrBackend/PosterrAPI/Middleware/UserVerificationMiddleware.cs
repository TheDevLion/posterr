using System.Text.Json;
using PosterrAPI.Models;
using PosterrBackend.Application.Interfaces;

public class UserVerificationMiddleware
{
    private readonly RequestDelegate _next;

    public UserVerificationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        context.Request.EnableBuffering();

        var endpoint = context.GetEndpoint();
        if (endpoint != null)
        {
            var attribute = endpoint.Metadata.GetMetadata<UserVerification>();
            if (attribute != null)
            {
                var requestBody = await new StreamReader(context.Request.Body).ReadToEndAsync();
                context.Request.Body.Seek(0, SeekOrigin.Begin);
                var requestData = JsonSerializer.Deserialize<UserVerificationRequestMiddleware>(requestBody);
                if (requestData != null && requestData.Creator != null)
                {
                    var username = requestData.Creator;

                    var userService = context.RequestServices.GetRequiredService<IUserService>();

                    if (string.IsNullOrEmpty(username) || !await userService.IsUserValid(username))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync($"User '{username}' not found.");
                        return;
                    }
                }

            }
        }

        await _next(context);
    }
}
