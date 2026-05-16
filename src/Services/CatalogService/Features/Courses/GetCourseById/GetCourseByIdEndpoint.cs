using EduFlow.Shared.Results;
using MediatR;

namespace CatalogService.Features.Courses.GetCourseById;

public class GetCourseByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/courses/{id}", async (string id, ISender sender) =>
        {
            var result = await sender.Send(new GetCourseByIdQuery(id));
            return result.ToHttpResult();
        });
    }
}
