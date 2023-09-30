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

    public virtual void AddPurchasedMovie(Movie movie, ExpirationDate expirationDate, Dollars price)
    {
        var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
        _purchasedMovies ??= new List<PurchasedMovie>();
        _purchasedMovies.Add(purchasedMovie);

        MoneySpent += price;
    }
}
