namespace Logic.Entities;

public class Movie : Entity
{
    public string Name { get; set; } = string.Empty;

    public LicensingModel LicensingModel { get; set; }
}
