namespace Logic.Entities;

public class Customer : Entity
{
    public required string Name { get; set; }
    public required string Email { get; set; }
    public CustomerStatus Status { get; set; }
    public DateTime? StatusExpirationDate { get; set; }
    public decimal MoneySpent { get; set; }
    public virtual List<PurchasedMovie>? PurchasedMovies { get; set; }
}
