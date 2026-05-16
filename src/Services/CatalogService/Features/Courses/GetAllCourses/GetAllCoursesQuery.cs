using CatalogService.Features.Courses.Shared;
using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Courses.GetAllCourses;

public record GetAllCoursesQuery : IRequest<Result<List<CourseResponse>>>;

public class GetAllCoursesQueryHandler : IRequestHandler<GetAllCoursesQuery, Result<List<CourseResponse>>>
{
    private readonly CatalogDbContext _context;

    public GetAllCoursesQueryHandler(CatalogDbContext context)
        => _context = context;

    public async Task<Result<List<CourseResponse>>> Handle(
        GetAllCoursesQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await _context.Courses.ToListAsync(cancellationToken);

        var response = courses.Select(c => new CourseResponse(
            c.Id,
            c.Name,
            c.Description,
            c.Price,
            c.ImageUrl,
            c.UserId,
            c.CategoryId,
            c.Features.Select(f => new FeatureResponse(f.Name)).ToList(),
            c.CreatedAt)).ToList();

        return response;
    }
}
