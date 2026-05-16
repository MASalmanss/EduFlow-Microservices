using BasketService.Domain.Entities;
using EduFlow.Shared.Results;
using EduFlow.Shared.Services;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace BasketService.Features.Baskets.AddBasketItem;

public record AddBasketItemCommand(
    Guid CourseId,
    string CourseName,
    string CoursePrice,
    string ImageUrl) : IRequest<Result>;

public class AddBasketItemCommandValidator : AbstractValidator<AddBasketItemCommand>
{
    public AddBasketItemCommandValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("CourseId is required.");

        RuleFor(x => x.CourseName)
            .NotEmpty().WithMessage("CourseName is required.");

        RuleFor(x => x.CoursePrice)
            .NotEmpty().WithMessage("CoursePrice is required.");
    }
}

public class AddBasketItemCommandHandler : IRequestHandler<AddBasketItemCommand, Result>
{
    private readonly IDistributedCache _cache;
    private readonly IIdentityService _identityService;

    public AddBasketItemCommandHandler(IDistributedCache cache, IIdentityService identityService)
    {
        _cache = cache;
        _identityService = identityService;
    }

    public async Task<Result> Handle(AddBasketItemCommand request, CancellationToken cancellationToken)
    {
        var userId = _identityService.GetUserId();
        var key = $"basket-{userId}";

        var json = await _cache.GetStringAsync(key, cancellationToken);

        var basket = json is not null
            ? JsonSerializer.Deserialize<Basket>(json)!
            : new Basket { UserId = userId };

        var existingItem = basket.Items.FirstOrDefault(i => i.CourseId == request.CourseId);
        if (existingItem is not null)
            return Error.Conflict("This course is already in the basket.");

        basket.Items.Add(new BasketItem
        {
            CourseId = request.CourseId,
            CourseName = request.CourseName,
            CoursePrice = request.CoursePrice,
            ImageUrl = request.ImageUrl
        });

        await _cache.SetStringAsync(key, JsonSerializer.Serialize(basket), cancellationToken);

        return Result.Success();
    }
}
