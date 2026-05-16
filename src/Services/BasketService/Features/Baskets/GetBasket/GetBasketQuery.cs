using BasketService.Domain.Entities;
using EduFlow.Shared.Results;
using EduFlow.Shared.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BasketService.Features.Baskets.GetBasket;

public record GetBasketQuery : IRequest<Result<Basket>>;

public class GetBasketQueryHandler : IRequestHandler<GetBasketQuery, Result<Basket>>
{
    private readonly IDistributedCache _cache;
    private readonly IIdentityService _identityService;

    public GetBasketQueryHandler(IDistributedCache cache, IIdentityService identityService)
    {
        _cache = cache;
        _identityService = identityService;
    }

    public async Task<Result<Basket>> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserId();
        var key = $"basket-{userId}";

        var json = await _cache.GetStringAsync(key, cancellationToken);
        if (json is null)
            return Error.NotFound("Basket not found.");

        var basket = JsonSerializer.Deserialize<Basket>(json)!;

        return basket;
    }
}
