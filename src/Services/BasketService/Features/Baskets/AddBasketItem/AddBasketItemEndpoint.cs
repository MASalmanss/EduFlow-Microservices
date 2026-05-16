using EduFlow.Shared.Results;
using MediatR;

namespace BasketService.Features.Baskets.AddBasketItem;

public class AddBasketItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/baskets", async (
            AddBasketItemCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToHttpResult();
        });
    }
}
