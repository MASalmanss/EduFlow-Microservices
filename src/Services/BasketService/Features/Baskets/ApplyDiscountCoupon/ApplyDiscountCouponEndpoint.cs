using EduFlow.Shared.Results;
using MediatR;

namespace BasketService.Features.Baskets.ApplyDiscountCoupon;

public class ApplyDiscountCouponEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/baskets/apply-coupon", async (
            ApplyDiscountCouponCommand command,
            ISender sender) =>
        {
            var result = await sender.Send(command);
            return result.ToHttpResult();
        });
    }
}
