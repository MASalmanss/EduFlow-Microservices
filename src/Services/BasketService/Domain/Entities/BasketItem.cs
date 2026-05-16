namespace BasketService.Domain.Entities;

public class BasketItem
{
    public Guid CourseId { get; set; }
    public string CourseName { get; set; } = default!;
    public string CoursePrice { get; set; } = default!;
    public string ImageUrl { get; set; } = default!;
}
