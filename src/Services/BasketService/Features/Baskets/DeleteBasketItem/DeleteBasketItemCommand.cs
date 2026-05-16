using BasketService.Domain.Entities;
using EduFlow.Shared.Results;
using EduFlow.Shared.Services;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BasketService.Features.Baskets.DeleteBasketItem;

public record DeleteBasketItemCommand(Guid CourseId) : IRequest<Result>;

public class DeleteBasketItemCommandHandler : IRequestHandler<DeleteBasketItemCommand, Result>
{
    private readonly IDistributedCache _cache;
    private readonly IIdentityService _identityService;

    public DeleteBasketItemCommandHandler(IDistributedCache cache, IIdentityService identityService)
    {
        _cache = cache;
        _identityService = identityService;
    }

    public async Task<Result> Handle(DeleteBasketItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserId();
        var key = $"basket-{userId}";

        var json = await _cache.GetStringAsync(key, cancellationToken);
        if (json is null)
            return Error.NotFound("Basket not found.");

        var basket = JsonSerializer.Deserialize<Basket>(json)!;

        var item = basket.Items.FirstOrDefault(i => i.CourseId == request.CourseId);
        if (item is null)
            return Error.NotFound($"Course with id '{request.CourseId}' was not found in the basket.");

        basket.Items.Remove(item);

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(basket), cancellationToken);

        return Result.Success();
    }
}
