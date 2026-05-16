using CatalogService.Features.Courses.Shared;
using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Courses.GetAllCoursesByUserId;

public record GetAllCoursesByUserIdQuery(string UserId) : IRequest<Result<List<CourseResponse>>>;

public class GetAllCoursesByUserIdQueryHandler
    : IRequestHandler<GetAllCoursesByUserIdQuery, Result<List<CourseResponse>>>
{
    private readonly CatalogDbContext _context;

    public GetAllCoursesByUserIdQueryHandler(CatalogDbContext context)
        => _context = context;

    public async Task<Result<List<CourseResponse>>> Handle(
        GetAllCoursesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var courses = await _context.Courses
            .Where(c => c.UserId == request.UserId)
            .ToListAsync(cancellationToken);

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
