using EduFlow.Shared.Results;
using MediatR;

namespace CatalogService.Features.Categories.GetCategoryById;

public class GetCategoryByIdEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/categories/{id}", async (string id, ISender sender) =>
        {
            var result = await sender.Send(new GetCategoryByIdQuery(id));
            return result.ToHttpResult();
        });
    }
}
