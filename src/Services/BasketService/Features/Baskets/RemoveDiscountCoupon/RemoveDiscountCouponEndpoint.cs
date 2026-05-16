using EduFlow.Shared.Results;
using MediatR;

namespace BasketService.Features.Baskets.RemoveDiscountCoupon;

public class RemoveDiscountCouponEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/baskets/remove-coupon", async (ISender sender) =>
        {
            var result = await sender.Send(new RemoveDiscountCouponCommand());
            return result.ToHttpResult();
        });
    }
}
