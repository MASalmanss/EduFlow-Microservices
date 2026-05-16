namespace CatalogService.Features.Courses.Shared;

public record FeatureResponse(string Name);

public record CourseResponse(
    string Id,
    string Name,
    string Description,
    decimal Price,
    string? ImageUrl,
    string UserId,
    string CategoryId,
    List<FeatureResponse> Features,
    DateTime CreatedAt);
