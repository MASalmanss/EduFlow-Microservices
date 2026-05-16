using EduFlow.Shared.Results;
using MediatR;

namespace CatalogService.Features.Courses.UpdateCourse;

public class UpdateCourseEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/courses/{id}", async (
            string id,
            UpdateCourseCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command with { Id = id });
            return result.ToHttpResult();
        });
    }
}
