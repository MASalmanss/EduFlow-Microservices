using EduFlow.Shared.Results;
using MediatR;

namespace CatalogService.Features.Courses.CreateCourse;

public class CreateCourseEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/courses", async (
            CreateCourseCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToHttpResult();
        });
    }
}
