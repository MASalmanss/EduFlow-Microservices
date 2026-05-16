using EduFlow.Shared.Results;
using MediatR;

namespace CatalogService.Features.Courses.GetAllCoursesByUserId;

public class GetAllCoursesByUserIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/courses/user/{userId}", async (string userId, ISender sender) =>
        {
            var result = await sender.Send(new GetAllCoursesByUserIdQuery(userId));
            return result.ToHttpResult();
        });
    }
}
