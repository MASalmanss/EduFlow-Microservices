using CatalogService.Infrastructure.Persistence;
using EduFlow.Shared.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Features.Categories.GetAllCategories;

public record CategoryResponse(string Id, string Name, DateTime CreatedAt);

public record GetAllCategoriesQuery : IRequest<Result<List<CategoryResponse>>>;

public class GetAllCategoriesQueryHandler : IRequestHandler<GetAllCategoriesQuery, Result<List<CategoryResponse>>>
{
    private readonly CatalogDbContext _context;

    public GetAllCategoriesQueryHandler(CatalogDbContext context)
        => _context = context;

    public async Task<Result<List<CategoryResponse>>> Handle(
        GetAllCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = await _context.Categories
            .ToListAsync(cancellationToken);

        var response = categories
            .Select(c => new CategoryResponse(c.Id, c.Name, c.CreatedAt))
            .ToList();

        return response;
    }
}
