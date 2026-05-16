using BasketService.Domain.Entities;
using BasketService.Features.Baskets.AddBasketItem;
using EduFlow.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text.Json;

namespace BasketService.Tests.Features;

public class AddBasketItemCommandHandlerTests
{
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IIdentityService> _identityMock;
    private readonly AddBasketItemCommandHandler _handler;
    private const string UserId = "test-user-1";

    public AddBasketItemCommandHandlerTests()
    {
        _cacheMock = new Mock<IDistributedCache>();
        _identityMock = new Mock<IIdentityService>();
        _identityMock.Setup(x => x.GetUserId()).Returns(UserId);
        _handler = new AddBasketItemCommandHandler(_cacheMock.Object, _identityMock.Object);
    }

    [Fact]
    public async Task Handle_WhenBasketIsEmpty_CreatesNewBasketAndAddsItem()
    {
        // Arrange
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        var command = new AddBasketItemCommand(Guid.NewGuid(), "ASP.NET Core", "199.99", "image.jpg");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _cacheMock.Verify(c => c.SetAsync(
            $"basket-{UserId}",
            It.IsAny<byte[]>(),
            It.IsAny<DistributedCacheEntryOptions>(),
            It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WhenCourseAlreadyInBasket_ReturnsConflictError()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var basket = new Basket
        {
            UserId = UserId,
            Items = [new BasketItem { CourseId = courseId, CourseName = "ASP.NET Core", CoursePrice = "199.99", ImageUrl = "image.jpg" }]
        };

        var json = JsonSerializer.SerializeToUtf8Bytes(basket);
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(json);

        var command = new AddBasketItemCommand(courseId, "ASP.NET Core", "199.99", "image.jpg");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(409, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenBasketExistsAndCourseIsNew_AddsItemToBasket()
    {
        // Arrange
        var basket = new Basket { UserId = UserId, Items = [] };
        var json = JsonSerializer.SerializeToUtf8Bytes(basket);
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(json);

        var command = new AddBasketItemCommand(Guid.NewGuid(), "Docker & K8s", "299.99", "image2.jpg");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
