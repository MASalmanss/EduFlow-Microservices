using CatalogService.Features.Categories.GetAllCategories;
using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Categories.GetCategoryById;

public record GetCategoryByIdQuery(string Id) : IRequest<Result<CategoryResponse>>;

public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, Result<CategoryResponse>>
{
    private readonly CatalogDbContext _context;

    public GetCategoryByIdQueryHandler(CatalogDbContext context)
        => _context = context;

    public async Task<Result<CategoryResponse>> Handle(
        GetCategoryByIdQuery request,
        CancellationToken cancellationToken)
    {
        var category = await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == request.Id, cancellationToken);

        if (category is null)
            return Error.NotFound($"Category with id '{request.Id}' was not found.");

        return new CategoryResponse(category.Id, category.Name, category.CreatedAt);
    }
}
