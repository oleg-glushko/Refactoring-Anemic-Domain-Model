namespace Logic.Entities;

public class Customer : Entity
{
    public required CustomerName Name { get; set; }
    public required Email Email { get; set; }
    public CustomerStatus Status { get; set; }
    public ExpirationDate StatusExpirationDate { get; set; } = null!;
    public Dollars MoneySpent { get; set; } = null!;
    public virtual List<PurchasedMovie>? PurchasedMovies { get; set; }
}
