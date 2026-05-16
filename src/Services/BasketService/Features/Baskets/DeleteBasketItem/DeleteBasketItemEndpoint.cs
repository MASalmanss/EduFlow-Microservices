using EduFlow.Shared.Results;
using MediatR;

namespace BasketService.Features.Baskets.DeleteBasketItem;

public class DeleteBasketItemEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/baskets/{courseId:guid}", async (Guid courseId, ISender sender) =>
        {
            var result = await sender.Send(new DeleteBasketItemCommand(courseId));
            return result.ToHttpResult();
        });
    }
}
