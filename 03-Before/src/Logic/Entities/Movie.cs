using System.Text.Json.Serialization;

namespace Logic.Entities;

public class Movie : Entity
{
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public LicensingModel LicensingModel { get; set; }
}
