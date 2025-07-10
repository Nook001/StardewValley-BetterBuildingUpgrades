using StardewValley;
using StardewModdingAPI;

namespace BetterBuildingUpgrades;

/// <summary>
/// Handles building detection functionality
/// </summary>
public static class BuildingDetector
{
    public static bool HasUpgradedGreenhouse() =>
        ModEntry.Config.EnableGreenhouseUpgrade && (HasBigGreenhouse() || HasDeluxeGreenhouse());


    public static bool HasBigGreenhouse() => HasBuilding("Big Greenhouse");

    public static bool HasDeluxeGreenhouse() => HasBuilding("Deluxe Greenhouse");

    public static bool HasRegularGreenhouse() => HasBuilding("Greenhouse");


    public static bool HasBigStable() =>
        ModEntry.Config.EnableStableUpgrade && HasBuilding("Big Stable");
    
    private static bool HasBuilding(string buildingType)
    {
        try
        {
            // Only check for buildings if the world is ready
            if (!Context.IsWorldReady || Game1.getFarm() == null)
                return false;
                
            foreach (var building in Game1.getFarm().buildings)
            {
                if (building.buildingType.Value == buildingType)
                {
                    return true;
                }
            }
            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }
}