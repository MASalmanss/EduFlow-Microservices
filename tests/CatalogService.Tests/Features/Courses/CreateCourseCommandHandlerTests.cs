using CatalogService.Domain.Entities;
using CatalogService.Features.Courses.CreateCourse;
using CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace CatalogService.Tests.Features.Courses;

public class CreateCourseCommandHandlerTests
{
    private readonly Mock<CatalogDbContext> _contextMock;
    private readonly CreateCourseCommandHandler _handler;

    public CreateCourseCommandHandlerTests()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>().Options;
        _contextMock = new Mock<CatalogDbContext>(options);
        _handler = new CreateCourseCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCategoryNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var categories = new List<Category>();
        var mockCategorySet = categories.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(c => c.Categories).Returns(mockCategorySet.Object);

        var command = new CreateCourseCommand(
            "ASP.NET Core", "Description", 199.99m,
            "user-1", "non-existent-category", null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenCategoryExists_ReturnsCourseId()
    {
        // Arrange
        var category = new Category("Backend");
        var categories = new List<Category> { category };
        var mockCategorySet = categories.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(c => c.Categories).Returns(mockCategorySet.Object);

        var courses = new List<Course>();
        var mockCourseSet = courses.AsQueryable().BuildMockDbSet();
        mockCourseSet.Setup(m => m.Add(It.IsAny<Course>()));
        _contextMock.Setup(c => c.Courses).Returns(mockCourseSet.Object);
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        var command = new CreateCourseCommand(
            "ASP.NET Core", "Description", 199.99m,
            "user-1", category.Id, null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Data);
    }
}
