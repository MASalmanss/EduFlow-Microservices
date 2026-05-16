using BasketService.Domain.Entities;
using BasketService.Features.Baskets.GetBasket;
using EduFlow.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text.Json;

namespace BasketService.Tests.Features;

public class GetBasketQueryHandlerTests
{
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IIdentityService> _identityMock;
    private readonly GetBasketQueryHandler _handler;
    private const string UserId = "test-user-1";

    public GetBasketQueryHandlerTests()
    {
        _cacheMock = new Mock<IDistributedCache>();
        _identityMock = new Mock<IIdentityService>();
        _identityMock.Setup(x => x.GetUserId()).Returns(UserId);
        _handler = new GetBasketQueryHandler(_cacheMock.Object, _identityMock.Object);
    }

    [Fact]
    public async Task Handle_WhenBasketNotFound_ReturnsNotFoundError()
    {
        // Arrange
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        // Act
        var result = await _handler.Handle(new GetBasketQuery(), CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenBasketExists_ReturnsBasket()
    {
        // Arrange
        var basket = new Basket
        {
            UserId = UserId,
            Items = [new BasketItem { CourseId = Guid.NewGuid(), CourseName = "ASP.NET Core", CoursePrice = "199.99", ImageUrl = "img.jpg" }]
        };
        var json = JsonSerializer.SerializeToUtf8Bytes(basket);
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(json);

        // Act
        var result = await _handler.Handle(new GetBasketQuery(), CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(UserId, result.Data!.UserId);
        Assert.Single(result.Data.Items);
    }
}
