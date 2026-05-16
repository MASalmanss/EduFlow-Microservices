using CatalogService.Features.Courses.Shared;
using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Courses.GetCourseById;

public record GetCourseByIdQuery(string Id) : IRequest<Result<CourseResponse>>;

public class GetCourseByIdQueryHandler : IRequestHandler<GetCourseByIdQuery, Result<CourseResponse>>
{
    private readonly CatalogDbContext _context;

    public GetCourseByIdQueryHandler(CatalogDbContext context)
        => _context = context;

    public async Task<Result<CourseResponse>> Handle(
        GetCourseByIdQuery request,
        CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course is null)
            return Error.NotFound($"Course with id '{request.Id}' was not found.");

        return new CourseResponse(
            course.Id,
            course.Name,
            course.Description,
            course.Price,
            course.ImageUrl,
            course.UserId,
            course.CategoryId,
            course.Features.Select(f => new FeatureResponse(f.Name)).ToList(),
            course.CreatedAt);
    }
}
