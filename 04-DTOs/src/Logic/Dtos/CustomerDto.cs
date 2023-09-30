namespace Logic.Dtos;

public class CustomerDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Status { get; set; }
    public DateTime? StatusExpirationDate { get; set; }
    public decimal MoneySpent { get; set; }
    public List<PurchasedMovieDto>? PurchasedMovies { get; set; }
}
