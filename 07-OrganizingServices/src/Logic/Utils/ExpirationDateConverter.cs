using Logic.Customers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Logic.Utils;

public class ExpirationDateConverter : ValueConverter<ExpirationDate, DateTime?>
{
#pragma warning disable EF1001 // Internal EF Core API usage.
    public ExpirationDateConverter() : base(
        v => (DateTime?)v,
        v => (ExpirationDate)v,
        // see https://github.com/dotnet/efcore/issues/13850
        convertsNulls: true)
    {
    }
#pragma warning restore EF1001 // Internal EF Core API usage.
}
