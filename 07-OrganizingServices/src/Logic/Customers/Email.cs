using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace Logic.Customers;

public class Email : Common.ValueObject<Email>
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string email)
    {
        email = email.Trim();

        if (email.Length == 0)
            return Result.Failure<Email>("Email should not be empty");

        if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
            return Result.Failure<Email>("Email is invalid");

        return Result.Success(new Email(email));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToUpperInvariant();
    }

    public static implicit operator string(Email email)
    {
        return email.Value;
    }

    public static explicit operator Email(string email)
    {
        return Create(email).Value;
    }
}