namespace CatalogService.Domain.Entities;

public class Category
{
    public string Id { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public DateTime CreatedAt { get; private set; }

    private Category() { }

    public Category(string name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name)
    {
        Name = name;
    }
}
