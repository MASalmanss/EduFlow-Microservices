using EduFlow.Shared.Results;
using MediatR;

namespace CatalogService.Features.Courses.DeleteCourse;

public class DeleteCourseEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/courses/{id}", async (string id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteCourseCommand(id));
            return result.ToHttpResult();
        });
    }
}
