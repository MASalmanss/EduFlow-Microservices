namespace CatalogService.Domain.Entities;

public class Course
{
    public string Id { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public decimal Price { get; private set; }
    public string? ImageUrl { get; private set; }
    public string UserId { get; private set; } = default!;
    public string CategoryId { get; private set; } = default!;
    public List<Feature> Features { get; private set; } = [];
    public DateTime CreatedAt { get; private set; }

    private Course() { }

    public Course(string name, string description, decimal price, string userId, string categoryId, string? imageUrl = null)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        Description = description;
        Price = price;
        UserId = userId;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, decimal price, string categoryId, string? imageUrl)
    {
        Name = name;
        Description = description;
        Price = price;
        CategoryId = categoryId;
        ImageUrl = imageUrl;
    }

    public void AddFeature(Feature feature) => Features.Add(feature);

    public void RemoveFeature(Feature feature) => Features.Remove(feature);
}
