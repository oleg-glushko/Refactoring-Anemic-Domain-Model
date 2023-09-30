using CSharpFunctionalExtensions;

namespace Logic.Customers;

public class ExpirationDate : Common.ValueObject<ExpirationDate>
{
    public static readonly ExpirationDate Infinite = new ExpirationDate(null);

    public DateTime? Date { get; }

    public bool IsExpired => this != Infinite && Date < DateTime.UtcNow;

    private ExpirationDate(DateTime? date)
    {
        Date = date;
    }

    public static Result<ExpirationDate> Create(DateTime date)
    {
        return Result.Success(new ExpirationDate(date));

    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Date ?? DateTime.MaxValue; // a dirty hack
    }

    public static explicit operator ExpirationDate(DateTime? date)
    {
        if (date.HasValue)
            return Create(date.Value).Value;

        return Infinite;
    }

    public static implicit operator DateTime?(ExpirationDate date)
    {
        return date.Date;
    }
}
