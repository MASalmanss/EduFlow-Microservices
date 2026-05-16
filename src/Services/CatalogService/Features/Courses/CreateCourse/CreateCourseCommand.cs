using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Courses.CreateCourse;

public record CreateCourseCommand(
    string Name,
    string Description,
    decimal Price,
    string UserId,
    string CategoryId,
    string? ImageUrl) : IRequest<Result<string>>;

public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
{
    public CreateCourseCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Course name is required.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.");

        RuleFor(x => x.Price)
            .GreaterThan(0).WithMessage("Price must be greater than 0.");

        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("UserId is required.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");
    }
}

public class CreateCourseCommandHandler : IRequestHandler<CreateCourseCommand, Result<string>>
{
    private readonly CatalogDbContext _context;

    public CreateCourseCommandHandler(CatalogDbContext context)
        => _context = context;

    public async Task<Result<string>> Handle(CreateCourseCommand request, CancellationToken cancellationToken)
    {
        var categoryExists = await _context.Categories
            .AnyAsync(c => c.Id == request.CategoryId, cancellationToken);

        if (!categoryExists)
            return Error.NotFound($"Category with id '{request.CategoryId}' was not found.");

        var course = new Course(
            request.Name,
            request.Description,
            request.Price,
            request.UserId,
            request.CategoryId,
            request.ImageUrl);

        _context.Courses.Add(course);
        await _context.SaveChangesAsync(cancellationToken);

        return course.Id;
    }
}
