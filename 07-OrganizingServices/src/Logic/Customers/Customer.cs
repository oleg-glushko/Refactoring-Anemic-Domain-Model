using Microsoft.EntityFrameworkCore.Infrastructure;
using Logic.Utils;
using CSharpFunctionalExtensions.ValueTasks;
using Logic.Movies;
using CSharpFunctionalExtensions;

namespace Logic.Customers;

public class Customer : Common.Entity
{
    private readonly ILazyLoader? _lazyLoader;

    public CustomerName Name { get; set; } = null!;
    private readonly string _email = string.Empty;
    public Email Email => (Email)_email;
    public Dollars MoneySpent { get; private set; } = null!;

    public CustomerStatus Status { get; private set; } = CustomerStatus.Regular;

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
        _email = email;

        MoneySpent = Dollars.Of(0);
        Status = CustomerStatus.Regular;

        _lazyLoader = lazyLoader;
    }

    public bool HasPurchasedMovie(Movie movie)
    {
        if (PurchasedMovie == null)
            return false;
        return PurchasedMovies!.Any(x =>
                x.Movie == movie && !x.ExpirationDate.IsExpired);
    }

    public void PurchasedMovie(Movie movie)
    {
        if (!HasPurchasedMovie(movie))
            throw new Exception();

        ExpirationDate expirationDate = movie.GetExpirationDate();
        Dollars price = movie.CalculatePrice(Status);

        var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
        _purchasedMovies ??= new List<PurchasedMovie>();
        _purchasedMovies.Add(purchasedMovie);

        MoneySpent += price;
    }

    public Result CanPromote()
    {
        if (Status.IsAdvanced)
            return Result.Failure("The customer is already has the Advanced status");

        if (PurchasedMovies?.Count(x =>
            x.ExpirationDate == ExpirationDate.Infinite ||
            x.ExpirationDate.Date == DateTime.UtcNow.AddDays(-30)) < 2)
            return Result.Failure("The customer has to have at least 2 active movies during the last 30 days");

        if (PurchasedMovies?.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1))
                    .Sum(x => x.Price) < 100m)
            return Result.Failure("The customer has to have at least 100 dollars spent during last year");

        return Result.Success();
    }

    public void Promote()
    {
        if (CanPromote().IsFailure)
            throw new Exception();

        Status = Status.Promote();
    }
}
