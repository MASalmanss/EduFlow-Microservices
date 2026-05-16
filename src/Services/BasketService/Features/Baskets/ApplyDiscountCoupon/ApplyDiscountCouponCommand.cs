using BasketService.Domain.Entities;
using EduFlow.Shared.Results;
using EduFlow.Shared.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BasketService.Features.Baskets.ApplyDiscountCoupon;

public record ApplyDiscountCouponCommand(string CouponCode, double DiscountRate) : IRequest<Result>;

public class ApplyDiscountCouponCommandValidator : AbstractValidator<ApplyDiscountCouponCommand>
{
    public ApplyDiscountCouponCommandValidator()
    {
        RuleFor(x => x.CouponCode)
            .NotEmpty().WithMessage("Coupon code is required.");

        RuleFor(x => x.DiscountRate)
            .InclusiveBetween(1, 100).WithMessage("Discount rate must be between 1 and 100.");
    }
}

public class ApplyDiscountCouponCommandHandler : IRequestHandler<ApplyDiscountCouponCommand, Result>
{
    private readonly IDistributedCache _cache;
    private readonly IIdentityService _identityService;

    public ApplyDiscountCouponCommandHandler(IDistributedCache cache, IIdentityService identityService)
    {
        _cache = cache;
        _identityService = identityService;
    }

    public async Task<Result> Handle(ApplyDiscountCouponCommand request, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserId();
        var key = $"basket-{userId}";

        var json = await _cache.GetStringAsync(key, cancellationToken);
        if (json is null)
            return Error.NotFound("Basket not found.");

        var basket = JsonSerializer.Deserialize<Basket>(json)!;

        basket.CouponCode = request.CouponCode;
        basket.DiscountRate = request.DiscountRate;

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(basket), cancellationToken);

        return Result.Success();
    }
}
