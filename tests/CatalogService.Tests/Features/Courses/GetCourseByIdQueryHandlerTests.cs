using CatalogService.Domain.Entities;
using CatalogService.Features.Courses.GetCourseById;
using CatalogService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace CatalogService.Tests.Features.Courses;

public class GetCourseByIdQueryHandlerTests
{
    private readonly Mock<CatalogDbContext> _contextMock;
    private readonly GetCourseByIdQueryHandler _handler;

    public GetCourseByIdQueryHandlerTests()
    {
        var options = new DbContextOptionsBuilder<CatalogDbContext>().Options;
        _contextMock = new Mock<CatalogDbContext>(options);
        _handler = new GetCourseByIdQueryHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_WhenCourseNotFound_ReturnsNotFoundError()
    {
        // Arrange
        var courses = new List<Course>();
        var mockSet = courses.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(c => c.Courses).Returns(mockSet.Object);

        var query = new GetCourseByIdQuery("non-existent-id");

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(404, result.Error!.Status);
    }

    [Fact]
    public async Task Handle_WhenCourseExists_ReturnsCourseResponse()
    {
        // Arrange
        var category = new Category("Backend");
        var course = new Course("ASP.NET Core", "Description", 199.99m, "user-1", category.Id);
        var courses = new List<Course> { course };
        var mockSet = courses.AsQueryable().BuildMockDbSet();
        _contextMock.Setup(c => c.Courses).Returns(mockSet.Object);

        var query = new GetCourseByIdQuery(course.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("ASP.NET Core", result.Data!.Name);
        Assert.Equal(199.99m, result.Data.Price);
    }
}
