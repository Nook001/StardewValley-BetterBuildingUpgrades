using StardewValley;
using StardewModdingAPI;

namespace BetterBuildingUpgrades;

/// <summary>
/// Handles building detection functionality
/// </summary>
public static class BuildingDetector
{
    // Greenhouse
    public static bool HasUpgradedGreenhouse() =>
        ModEntry.Config.EnableGreenhouseUpgrade && (HasBigGreenhouse() || HasDeluxeGreenhouse());

    public static bool HasBigGreenhouse() => HasBuilding("Big Greenhouse");

    public static bool HasDeluxeGreenhouse() => HasBuilding("Deluxe Greenhouse");

    public static bool HasRegularGreenhouse() => HasBuilding("Greenhouse");

    // Stable
    public static bool HasBigStable() =>
        ModEntry.Config.EnableStableUpgrade && HasBuilding("Big Stable");


    // Silo todo
    public static bool HasGrindingSilo() => true;
    
    private static bool HasBuilding(string buildingType)
    {
        try
        {
            // Only check for buildings if the world is ready
            if (!Context.IsWorldReady || Game1.getFarm() == null)
                return false;
            
            // Check if the farm has given building
            return Game1.getFarm().buildings.Any(b => b.buildingType.Value == buildingType);
        }
        catch (Exception)
        {
            return false;
        }
    }
}