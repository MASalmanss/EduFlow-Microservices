using EduFlow.Shared.Results;
using MediatR;

namespace BasketService.Features.Baskets.GetBasket;

public class GetBasketEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/baskets", async (ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery());
            return result.ToHttpResult();
        });
    }
}
