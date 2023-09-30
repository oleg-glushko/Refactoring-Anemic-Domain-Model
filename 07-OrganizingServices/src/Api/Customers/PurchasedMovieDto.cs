namespace Api.Customers;

public class PurchasedMovieDto
{
    public required MovieDto Movie { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
