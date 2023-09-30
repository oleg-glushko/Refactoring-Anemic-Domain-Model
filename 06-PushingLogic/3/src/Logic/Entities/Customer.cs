using Microsoft.EntityFrameworkCore.Infrastructure;
using Logic.Utils;

namespace Logic.Entities;

public class Customer : Entity
{
    private readonly ILazyLoader? _lazyLoader;

    public CustomerName Name { get; set; } = null!;
    public Email Email { get; private set; } = null!;
    public Dollars MoneySpent { get; private set; } = null!;

    public CustomerStatus Status { get; set; } = CustomerStatus.Regular;

    private IList<PurchasedMovie>? _purchasedMovies;
    private IList<PurchasedMovie>? InternalPurchasedMovies => _lazyLoader.Load(
        this, ref _purchasedMovies, nameof(PurchasedMovies));
    public virtual IReadOnlyList<PurchasedMovie>? PurchasedMovies => InternalPurchasedMovies?.AsReadOnly();

    private Customer()
    {
        _purchasedMovies = new List<PurchasedMovie>();
    }

    public Customer(CustomerName name, Email email, ILazyLoader? lazyLoader = null) : this()
    {
        Name = name;
        Email = email;

        MoneySpent = Dollars.Of(0);
        Status = CustomerStatus.Regular;

        _lazyLoader = lazyLoader;
    }

    public void PurchasedMovie(Movie movie)
    {
        ExpirationDate expirationDate = movie.GetExpirationDate();
        Dollars price = movie.CalculatePrice(Status);

        var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
        _purchasedMovies ??= new List<PurchasedMovie>();
        _purchasedMovies.Add(purchasedMovie);

        MoneySpent += price;
    }

    public bool Promote()
    {
        // at least 2 active movies during the last 30 days
        if (PurchasedMovies?.Count(x =>
                x.ExpirationDate == ExpirationDate.Infinite ||
                x.ExpirationDate.Date == DateTime.UtcNow.AddDays(-30)) < 2)
            return false;

        // at least 100 dollars spent during last year
        if (PurchasedMovies?.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1))
                    .Sum(x => x.Price) < 100m)
            return false;

        Status = Status.Promote();

        return true;
    }
}
