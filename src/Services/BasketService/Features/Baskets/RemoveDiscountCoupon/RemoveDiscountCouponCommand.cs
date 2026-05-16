using BasketService.Domain.Entities;
using EduFlow.Shared.Results;
using EduFlow.Shared.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BasketService.Features.Baskets.RemoveDiscountCoupon;

public record RemoveDiscountCouponCommand : IRequest<Result>;

public class RemoveDiscountCouponCommandHandler : IRequestHandler<RemoveDiscountCouponCommand, Result>
{
    private readonly IDistributedCache _cache;
    private readonly IIdentityService _identityService;

    public RemoveDiscountCouponCommandHandler(IDistributedCache cache, IIdentityService identityService)
    {
        _cache = cache;
        _identityService = identityService;
    }

    public async Task<Result> Handle(RemoveDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserId();
        var key = $"basket-{userId}";

        var json = await _cache.GetStringAsync(key, cancellationToken);
        if (json is null)
            return Error.NotFound("Basket not found.");

        var basket = JsonSerializer.Deserialize<Basket>(json)!;

        basket.CouponCode = null;
        basket.DiscountRate = null;

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(basket), cancellationToken);

        return Result.Success();
    }
}
