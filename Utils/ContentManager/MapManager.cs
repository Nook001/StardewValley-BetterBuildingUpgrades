using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Buildings;

namespace BetterBuildingUpgrades;

public static partial class ContentManager
{
    // Load the custom map
    public static void LoadMaps(object? sender, AssetRequestedEventArgs e)
    {
        if (!e.NameWithoutLocale.IsEquivalentTo("Maps/Greenhouse")) { return; }
        
        string mapPath;
        // Load greenhouse map based on upgrade applied and if using frontier farm
        if (BuildingDetector.HasDeluxeGreenhouse())
        {
            mapPath = ModEntry.Config.FrontierFarmCompatibilityMode ? 
            "assets/Deluxe_Greenhouse_FrontierFarm.tmx" : "assets/Deluxe_Greenhouse.tmx";
        }
        else if (BuildingDetector.HasBigGreenhouse())
        {
            mapPath = ModEntry.Config.FrontierFarmCompatibilityMode ? 
            "assets/Big_Greenhouse_FrontierFarm.tmx" : "assets/Big_Greenhouse.tmx";
        }
        else 
        { 
            return; 
        }

        e.LoadFromModFile<xTile.Map>(mapPath, AssetLoadPriority.High);
    }
}