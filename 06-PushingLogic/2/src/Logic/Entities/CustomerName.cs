using CSharpFunctionalExtensions;

namespace Logic.Entities;

public class CustomerName : ValueObject<CustomerName>
{
    public string Value { get; }

    private CustomerName(string value)
    {
        Value = value;
    }

    public static Result<CustomerName> Create(string customerName)
    {
        customerName = customerName.Trim();

        if (customerName.Length == 0 )
            return Result.Failure<CustomerName>("Customer name should not be empty");

        if (customerName.Length > 100)
            return Result.Failure<CustomerName>("Customer name is too long");

        return Result.Success(new CustomerName(customerName));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToUpperInvariant();
    }

    public static implicit operator string(CustomerName customerName)
    {
        return customerName.Value;
    }

    public static explicit operator CustomerName(string customerName)
    {
        return Create(customerName).Value;
    }
}
