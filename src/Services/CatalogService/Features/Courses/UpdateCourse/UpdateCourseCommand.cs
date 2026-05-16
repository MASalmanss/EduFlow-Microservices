using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Courses.UpdateCourse;

public record UpdateCourseCommand(
    string Id,
    string Name,
    string Description,
    decimal Price,
    string CategoryId,
    string? ImageUrl) : IRequest<Result>;

public class UpdateCourseCommandValidator : AbstractValidator<UpdateCourseCommand>
{
    public UpdateCourseCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Course id is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Course name is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");
    }
}

public class UpdateCourseCommandHandler : IRequestHandler<UpdateCourseCommand, Result>
{
    private readonly CatalogDbContext _context;

    public UpdateCourseCommandHandler(CatalogDbContext context)
        => _context = context;

    public async Task<Result> Handle(UpdateCourseCommand request, CancellationToken cancellationToken)
    {
        var course = await _context.Courses
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (course is null)
            return Error.NotFound($"Course with id '{request.Id}' was not found.");

        course.Update(request.Name, request.Description, request.Price, request.CategoryId, request.ImageUrl);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
