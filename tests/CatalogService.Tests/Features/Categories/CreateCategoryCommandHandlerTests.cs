using CatalogService.Domain.Entities;
using CatalogService.Features.Categories.CreateCategory;
using CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace CatalogService.Tests.Features.Categories;

public class CreateCategoryCommandHandlerTests
{
    private readonly Mock<CatalogDbContext> _contextMock;
    private readonly CreateCategoryCommandHandler _handler;

    public CreateCategoryCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>().Options;
        _contextMock = new Mock<CatalogDbContext>(options);
        _handler = new CreateCategoryCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryNameAlreadyExists_ReturnsConflictError()
    {
        // Arrange
        var existing = new List<Category> { new("Backend") };
        var mockSet = existing.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(c => c.Categories).Returns(mockSet.Object);

        var command = new CreateCategoryCommand("Backend");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(409, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenCategoryNameIsUnique_ReturnsSuccess()
    {
        // Arrange
        var existing = new List<Category>();
        var mockSet = existing.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(c => c.Categories).Returns(mockSet.Object);
        mockSet.Setup(m => m.Add(It.IsAny<Category>()));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateCategoryCommand("Frontend");

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }
}
