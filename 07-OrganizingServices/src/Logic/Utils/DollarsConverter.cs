using Logic.Customers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Logic.Utils;

public class DollarsConverter : ValueConverter<Dollars, decimal>
{
    public DollarsConverter() : base(
        v => (decimal)v,
        v => Dollars.Of(v))
    {
    }
}
