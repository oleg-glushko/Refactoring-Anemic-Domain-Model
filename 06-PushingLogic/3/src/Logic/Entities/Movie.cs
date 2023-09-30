namespace Logic.Entities;

public class Movie : Entity
{
    public string Name { get; private set; } = string.Empty;

    public LicensingModel LicensingModel { get; private set; }

    public ExpirationDate GetExpirationDate()
    {
        return LicensingModel switch
        {
            LicensingModel.TwoDays => (ExpirationDate)DateTime.UtcNow.AddDays(2),
            LicensingModel.LifeLong => ExpirationDate.Infinite,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    public Dollars CalculatePrice(CustomerStatus status)
    {
        decimal modifier = 1 - status.GetDiscount();
        return LicensingModel switch
        {
            LicensingModel.TwoDays => Dollars.Of(4) * modifier,
            LicensingModel.LifeLong => Dollars.Of(8) * modifier,
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
