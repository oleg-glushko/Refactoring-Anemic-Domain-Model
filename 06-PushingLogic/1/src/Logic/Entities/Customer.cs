using Microsoft.EntityFrameworkCore.Infrastructure;
using Logic.Utils;
using Castle.Core.Resource;

namespace Logic.Entities;

public class Customer : Entity
{
    public CustomerName Name { get; set; } = null!;
    public Email Email { get; set; } = null!;
    public CustomerStatus Status { get; set; }
    public ExpirationDate StatusExpirationDate { get; set; } = null!;
    public Dollars MoneySpent { get; private set; } = null!;

    private readonly ILazyLoader? _lazyLoader;
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
        StatusExpirationDate = ExpirationDate.Infinite;

        _lazyLoader = lazyLoader;
    }

    public virtual void AddPurchasedMovie(Movie movie, ExpirationDate expirationDate, Dollars price)
    {
        var purchasedMovie = new PurchasedMovie
        {
            MovieId = movie.Id,
            CustomerId = Id,
            ExpirationDate = expirationDate,
            Price = price,
            PurchaseDate = DateTime.UtcNow
        };

        _purchasedMovies ??= new List<PurchasedMovie>();
        _purchasedMovies.Add(purchasedMovie);
        MoneySpent += price;
    }
}
