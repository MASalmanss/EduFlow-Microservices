namespace BasketService.Domain.Entities;

public class Basket
{
    public string UserId { get; set; } = default!;
    public List<BasketItem> Items { get; set; } = [];
    public string? CouponCode { get; set; }
    public double? DiscountRate { get; set; }
}
