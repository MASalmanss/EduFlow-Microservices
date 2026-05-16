using BasketService.Domain.Entities;
using BasketService.Features.Baskets.ApplyDiscountCoupon;
using BasketService.Features.Baskets.RemoveDiscountCoupon;
using EduFlow.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text.Json;

namespace BasketService.Tests.Features;

public class ApplyDiscountCouponCommandHandlerTests
{
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IIdentityService> _identityMock;
    private readonly ApplyDiscountCouponCommandHandler _applyHandler;
    private readonly RemoveDiscountCouponCommandHandler _removeHandler;
    private const string UserId = "test-user-1";

    public ApplyDiscountCouponCommandHandlerTests()
    {
        _cacheMock = new Mock<IDistributedCache>();
        _identityMock = new Mock<IIdentityService>();
        _identityMock.Setup(x => x.GetUserId()).Returns(UserId);
        _applyHandler = new ApplyDiscountCouponCommandHandler(_cacheMock.Object, _identityMock.Object);
        _removeHandler = new RemoveDiscountCouponCommandHandler(_cacheMock.Object, _identityMock.Object);
    }

    [Fact]
    public async Task ApplyCoupon_WhenBasketNotFound_ReturnsNotFoundError()
    {
        // Arrange
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        // Act
        var result = await _applyHandler.Handle(new ApplyDiscountCouponCommand("SAVE20", 20), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error!.Status);
    }

    [Fact]
    public async Task ApplyCoupon_WhenBasketExists_SetsCouponAndDiscountRate()
    {
        // Arrange
        var basket = new Basket { UserId = UserId, Items = [] };
        var json = JsonSerializer.SerializeToUtf8Bytes(basket);
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(json);

        // Act
        var result = await _applyHandler.Handle(new ApplyDiscountCouponCommand("SAVE20", 20), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _cacheMock.Verify(c => c.SetAsync(
            $"basket-{UserId}",
            It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RemoveCoupon_WhenBasketExists_ClearsCouponAndDiscountRate()
    {
        // Arrange
        var basket = new Basket { UserId = UserId, Items = [], CouponCode = "SAVE20", DiscountRate = 20 };
        var json = JsonSerializer.SerializeToUtf8Bytes(basket);
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(json);

        // Act
        var result = await _removeHandler.Handle(new RemoveDiscountCouponCommand(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
