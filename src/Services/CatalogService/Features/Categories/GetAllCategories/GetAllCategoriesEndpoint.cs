using EduFlow.Shared.Results;
using MediatR;

namespace CatalogService.Features.Categories.GetAllCategories;

public class GetAllCategoriesEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories", async (ISender sender) =>
        {
            var result = await sender.Send(new GetAllCategoriesQuery());
            return result.ToHttpResult();
        });
    }
}
