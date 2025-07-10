using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Buildings;

namespace BetterBuildingUpgrades;

/// <summary>
/// Handles asset loading for the mod
/// </summary>
public static class AssetManager
{
    // Load the custom textures for the buildings
    public static void LoadTextures(object? sender, AssetRequestedEventArgs e)
    {
        var textureMappings = new Dictionary<string, string>
        {
            { "Buildings/Big Silo", "assets/Big_Silo.png" },
            { "Buildings/Deluxe Silo", "assets/Deluxe_Silo.png" },
            { "Buildings/Big Greenhouse", "assets/Big_Greenhouse.png" },
            { "Buildings/Deluxe Greenhouse", "assets/Deluxe_Greenhouse.png" },
            { "Buildings/Big Well", "assets/Big_Well.png" },
            { "Buildings/Big Stable", "assets/Big_Stable.png" },
        };

        if (textureMappings.TryGetValue(e.NameWithoutLocale.BaseName, out string? filePath))
        {
            e.LoadFromModFile<Microsoft.Xna.Framework.Graphics.Texture2D>(filePath, AssetLoadPriority.Medium);
        }
    }

    // Load the custom buildings
    public static void LoadBuildings(object? sender, AssetRequestedEventArgs e)
    {
        if (e.NameWithoutLocale.IsEquivalentTo("Data/Buildings"))
        {
            e.Edit(asset =>
            {
                var data = asset.AsDictionary<string, BuildingData>().Data;

                if (ModEntry.Config.EnableGreenhouseUpgrade)
                {
                    data["Big Greenhouse"] = BuildingManager.BigGreenHouse(ModEntry.Config);
                    data["Deluxe Greenhouse"] = BuildingManager.DeluxeGreenHouse(ModEntry.Config);
                }

                if (ModEntry.Config.EnableSiloUpgrade)
                {
                    data["Big Silo"] = BuildingManager.BigSilo(ModEntry.Config);
                    data["Deluxe Silo"] = BuildingManager.DeluxeSilo(ModEntry.Config);
                }

                if (ModEntry.Config.EnableWellUpgrade)
                {
                    data["Big Well"] = BuildingManager.BigWell(ModEntry.Config);
                }

                if (ModEntry.Config.EnableStableUpgrade)
                {
                    data["Big Stable"] = BuildingManager.BigStable(ModEntry.Config, data["Stable"]);
                }
            });
        }
    }
    
    // Load the custom greenhouse map
    public static void LoadGreenhouseMap(object? sender, AssetRequestedEventArgs e)
    {
        if (e.NameWithoutLocale.IsEquivalentTo("Maps/Greenhouse"))
        {
            if (BuildingDetector.HasDeluxeGreenhouse())
            {
                if (ModEntry.Config.FrontierFarmCompatibilityMode)
                {
                    e.LoadFromModFile<xTile.Map>("assets/Deluxe_Greenhouse_FrontierFarm.tmx", AssetLoadPriority.High);
                }
                else
                {
                    e.LoadFromModFile<xTile.Map>("assets/Deluxe_Greenhouse.tmx", AssetLoadPriority.High);
                }
            }
            else if (BuildingDetector.HasBigGreenhouse())
            {
                if (ModEntry.Config.FrontierFarmCompatibilityMode)
                {
                    e.LoadFromModFile<xTile.Map>("assets/Big_Greenhouse_FrontierFarm.tmx", AssetLoadPriority.High);
                }
                else
                {
                    e.LoadFromModFile<xTile.Map>("assets/Big_Greenhouse.tmx", AssetLoadPriority.High);
                }
            }
        }
    }
}