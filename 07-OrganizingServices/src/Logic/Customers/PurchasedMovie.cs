using Logic.Common;
using Logic.Movies;

namespace Logic.Customers;

public class PurchasedMovie : Entity
{
    public virtual Movie Movie { get; private set; } = null!;
    public virtual Customer Customer { get; private set; } = null!;

    public Dollars Price { get; private set; } = Dollars.Of(0);
    public DateTime PurchaseDate { get; private set; }
    public ExpirationDate ExpirationDate { get; private set; } = ExpirationDate.Infinite;

    protected PurchasedMovie()
    {
    }

    internal PurchasedMovie(Movie movie, Customer customer, Dollars price, ExpirationDate expirationDate)
    {
        if (price.IsZero)
            throw new ArgumentException(nameof(price));

        if (expirationDate.IsExpired)
            throw new ArgumentException(nameof(expirationDate));

        Movie = movie;
        Customer = customer;
        Price = price;
        ExpirationDate = expirationDate;
        PurchaseDate = DateTime.UtcNow;
    }
}
