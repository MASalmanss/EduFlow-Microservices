using CatalogService.Domain.Entities;
using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Results;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Categories.CreateCategory;

public record CreateCategoryCommand(string Name) : IRequest<Result<string>>;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
    public CreateCategoryCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required.")
            .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");
    }
}

public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<string>>
{
    private readonly CatalogDbContext _context;

    public CreateCategoryCommandHandler(CatalogDbContext context)
        => _context = context;

    public async Task<Result<string>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var exists = await _context.Categories
            .AnyAsync(c => c.Name == request.Name, cancellationToken);

        if (exists)
            return Error.Conflict($"A category named '{request.Name}' already exists.");

        var category = new Category(request.Name);

        _context.Categories.Add(category);
        await _context.SaveChangesAsync(cancellationToken);

        return category.Id;
    }
}
