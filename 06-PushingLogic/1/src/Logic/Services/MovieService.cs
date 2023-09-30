using Logic.Entities;

namespace Logic.Services;

public class MovieService
{
    public ExpirationDate GetExpirationDate(LicensingModel licensingModel)
    {
        ExpirationDate result = licensingModel switch
        {
            LicensingModel.TwoDays => (ExpirationDate)DateTime.UtcNow.AddDays(2),
            LicensingModel.LifeLong => ExpirationDate.Infinite,
            _ => throw new ArgumentOutOfRangeException(),
        };

        return result;
    }
}
