using CatalogService.Domain.Entities;
using CatalogService.Features.Categories.GetCategoryById;
using CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace CatalogService.Tests.Features.Categories;

public class GetCategoryByIdQueryHandlerTests
{
    private readonly Mock<CatalogDbContext> _contextMock;
    private readonly GetCategoryByIdQueryHandler _handler;

    public GetCategoryByIdQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>().Options;
        _contextMock = new Mock<CatalogDbContext>(options);
        _handler = new GetCategoryByIdQueryHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var categories = new List<Category>();
        var mockSet = categories.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(c => c.Categories).Returns(mockSet.Object);

        var query = new GetCategoryByIdQuery("non-existent-id");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenCategoryExists_ReturnsCategoryResponse()
    {
        // Arrange
        var category = new Category("Backend");
        var categories = new List<Category> { category };
        var mockSet = categories.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(c => c.Categories).Returns(mockSet.Object);

        var query = new GetCategoryByIdQuery(category.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Backend", result.Data!.Name);
        Assert.Equal(category.Id, result.Data.Id);
    }
}
