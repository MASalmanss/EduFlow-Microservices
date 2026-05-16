using BasketService.Domain.Entities;
using BasketService.Features.Baskets.DeleteBasketItem;
using EduFlow.Shared.Services;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using System.Text.Json;

namespace BasketService.Tests.Features;

public class DeleteBasketItemCommandHandlerTests
{
    private readonly Mock<IDistributedCache> _cacheMock;
    private readonly Mock<IIdentityService> _identityMock;
    private readonly DeleteBasketItemCommandHandler _handler;
    private const string UserId = "test-user-1";

    public DeleteBasketItemCommandHandlerTests()
    {
        _cacheMock = new Mock<IDistributedCache>();
        _identityMock = new Mock<IIdentityService>();
        _identityMock.Setup(x => x.GetUserId()).Returns(UserId);
        _handler = new DeleteBasketItemCommandHandler(_cacheMock.Object, _identityMock.Object);
    }

    [Fact]
    public async Task Handle_WhenBasketNotFound_ReturnsNotFoundError()
    {
        // Arrange
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);

        var command = new DeleteBasketItemCommand(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenCourseNotInBasket_ReturnsNotFoundError()
    {
        // Arrange
        var basket = new Basket { UserId = UserId, Items = [] };
        var json = JsonSerializer.SerializeToUtf8Bytes(basket);
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(json);

        var command = new DeleteBasketItemCommand(Guid.NewGuid());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenCourseExistsInBasket_RemovesItemAndReturnsSuccess()
    {
        // Arrange
        var courseId = Guid.NewGuid();
        var basket = new Basket
        {
            UserId = UserId,
            Items = [new BasketItem { CourseId = courseId, CourseName = "ASP.NET Core", CoursePrice = "199.99", ImageUrl = "img.jpg" }]
        };
        var json = JsonSerializer.SerializeToUtf8Bytes(basket);
        _cacheMock.Setup(c => c.GetAsync($"basket-{UserId}", It.IsAny<CancellationToken>()))
            .ReturnsAsync(json);

        var command = new DeleteBasketItemCommand(courseId);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
    }
}
