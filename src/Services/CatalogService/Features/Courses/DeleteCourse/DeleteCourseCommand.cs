using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Courses.DeleteCourse;

public record DeleteCourseCommand(string Id) : IRequest<Result>;

public class DeleteCourseCommandHandler : IRequestHandler<DeleteCourseCommand, Result>
{
    private readonly CatalogDbContext _context;

    public DeleteCourseCommandHandler(CatalogDbContext context)
        => _context = context;

    public async Task<Result> Handle(DeleteCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course is null)
            return Error.NotFound($"Course with id '{request.Id}' was not found.");

        _context.Courses.Remove(course);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
