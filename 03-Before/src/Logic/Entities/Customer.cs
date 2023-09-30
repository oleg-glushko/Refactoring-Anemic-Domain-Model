using System.ComponentModel.DataAnnotations;

namespace Logic.Entities;

public class Customer : Entity
{
    [MaxLength(100, ErrorMessage = "Name is too long")]
    public required string Name { get; set; }

    [EmailAddress]
    public required string Email { get; set; }

    public CustomerStatus Status { get; set; }

    public DateTime? StatusExpirationDate { get; set; }

    public decimal MoneySpent { get; set; }

    public virtual List<PurchasedMovie>? PurchasedMovies { get; set; }
}
