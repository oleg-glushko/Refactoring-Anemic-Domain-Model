namespace Logic.Entities;

public class PurchasedMovie : Entity
{
    public long MovieId { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public long CustomerId { get; set; }

    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
