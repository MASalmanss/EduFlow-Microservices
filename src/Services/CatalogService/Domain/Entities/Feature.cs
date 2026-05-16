namespace CatalogService.Domain.Entities;

public class Feature
{
    public string Name { get; private set; } = default!;

    private Feature() { }

    public Feature(string name)
    {
        Name = name;
    }
}
