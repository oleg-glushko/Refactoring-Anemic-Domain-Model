using System.Text.Json.Serialization;

namespace Logic.Entities;

public class PurchasedMovie : Entity
{
    [JsonIgnore]
    public long MovieId { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    [JsonIgnore]
    public long CustomerId { get; set; }

    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public DateTime? ExpirationDate { get; set; }
}
