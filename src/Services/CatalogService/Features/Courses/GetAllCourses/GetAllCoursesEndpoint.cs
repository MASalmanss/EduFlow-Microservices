using EduFlow.Shared.Results;
using MediatR;

namespace CatalogService.Features.Courses.GetAllCourses;

public class GetAllCoursesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/courses", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllCoursesQuery());
            return result.ToHttpResult();
        });
    }
}
