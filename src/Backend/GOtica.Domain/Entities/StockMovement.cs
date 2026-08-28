using GOtica.Domain.Enums;

namespace GOtica.Domain.Entities;

public class StockMovement
{
    public Guid Id { get; set; } = Guid.CreateVersion7();

    public int QuantityChange { get; set; }
    public StockMovementType Type { get; set; }

    public string? Reason { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public Guid UserId { get; set; }
    public User User { get; set; } = default!;
}
