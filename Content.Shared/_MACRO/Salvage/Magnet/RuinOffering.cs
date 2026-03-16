namespace Content.Shared.Salvage.Magnet;

/// <summary>
/// Ruin offered for the magnet, generated from station maps.
/// </summary>
public record struct RuinOffering : ISalvageMagnetOffering
{
    public RuinMapPrototype RuinMap;
    
    /// <summary>
    /// Generated name for the ruined station
    /// </summary>
    public string StationName;

    public RuinOffering()
    {
        RuinMap = null!;
        StationName = string.Empty;
    }
}

