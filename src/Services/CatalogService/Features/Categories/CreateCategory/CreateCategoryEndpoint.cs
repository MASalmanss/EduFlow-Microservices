using EduFlow.Shared.Results;
using MediatR;

namespace CatalogService.Features.Categories.CreateCategory;

public class CreateCategoryEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/categories", async (
            CreateCategoryCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToHttpResult();
        });
    }
}
