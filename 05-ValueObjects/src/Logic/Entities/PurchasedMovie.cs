namespace Logic.Entities;

public class PurchasedMovie : Entity
{
    public long MovieId { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public long CustomerId { get; set; }

    public required Dollars Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public required ExpirationDate ExpirationDate { get; set; }
}
