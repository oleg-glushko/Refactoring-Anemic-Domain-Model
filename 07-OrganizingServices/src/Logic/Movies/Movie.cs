using Logic.Common;
using Logic.Customers;

namespace Logic.Movies;

public abstract class Movie : Entity
{
    public string Name { get; private set; } = string.Empty;

    private LicensingModel LicensingModel { get; set; } // entity discriminator

    public abstract ExpirationDate GetExpirationDate();

    public Dollars CalculatePrice(CustomerStatus status)
    {
        decimal modifier = 1 - status.GetDiscount();
        return GetBasePrice() * modifier;
    }

    protected abstract Dollars GetBasePrice();
}

public class TwoDaysMovie : Movie
{
    public override ExpirationDate GetExpirationDate()
    {
        return (ExpirationDate)DateTime.UtcNow.AddDays(2);
    }

    protected override Dollars GetBasePrice()
    {
        return Dollars.Of(4);
    }
}

public class LifeLongMovie : Movie
{
    public override ExpirationDate GetExpirationDate()
    {
        return ExpirationDate.Infinite;
    }

    protected override Dollars GetBasePrice()
    {
        return Dollars.Of(8);
    }

}
