namespace Logic.Entities;

public class CustomerStatus : ValueObject<CustomerStatus>
{
    public static readonly CustomerStatus Regular = new(CustomerStatusType.Regular, ExpirationDate.Infinite);

    public CustomerStatusType Type { get; set; }
    public ExpirationDate ExpirationDate { get; private set; } = ExpirationDate.Infinite;

    public bool IsAdvanced => Type == CustomerStatusType.Advanced && !ExpirationDate.IsExpired;

    protected CustomerStatus()
    {
    }

    private CustomerStatus(CustomerStatusType type, ExpirationDate expirationDate) : this()
    {
        Type = type;
        ExpirationDate = expirationDate;
    }

    public CustomerStatus Promote() {
        return new CustomerStatus(CustomerStatusType.Advanced, (ExpirationDate)DateTime.UtcNow.AddYears(1));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Type;
        yield return ExpirationDate;
    }
}

public enum CustomerStatusType
{
    Regular = 1,
    Advanced = 2
}