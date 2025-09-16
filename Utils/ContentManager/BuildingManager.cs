using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Buildings;

namespace BetterBuildingUpgrades;

public static partial class ContentManager
{
    // Add new upgrade buildings data to the game
    private static void Add(IDictionary<string, BuildingData> data, string name, bool enabled, Func<ModConfig, BuildingData> builder) 
    {
        if (enabled) { data[name] = builder(ModEntry.Config); }
    }

    // Add new upgrade buildings data to the game with a base building data
    private static void AddWithBase(IDictionary<string, BuildingData> data, string name, bool enabled, string baseName, Func<ModConfig, BuildingData, BuildingData> builder) 
    {
        if (enabled) { data[name] = builder(ModEntry.Config, data[baseName]); }
    }

    // Load the custom buildings
    public static void LoadBuildings(object? sender, AssetRequestedEventArgs e)
    {
        if (e.NameWithoutLocale.IsEquivalentTo("Data/Buildings"))
        {
            e.Edit(asset =>
            {
                var data = asset.AsDictionary<string, BuildingData>().Data;

                // Green house upgrades
                Add(data, "Big Greenhouse", ModEntry.Config.EnableGreenhouseUpgrade, DataManager.BigGreenHouse);
                Add(data, "Deluxe Greenhouse", ModEntry.Config.EnableGreenhouseUpgrade, DataManager.DeluxeGreenHouse);
            
                // Silo upgrades
                Add(data, "Big Silo", ModEntry.Config.EnableSiloUpgrade, DataManager.BigSilo);
                Add(data, "Deluxe Silo", ModEntry.Config.EnableSiloUpgrade, DataManager.DeluxeSilo);
                Add(data, "Grinding Silo", ModEntry.Config.EnableSiloUpgrade, DataManager.GrindingSilo);
                
                // Well upgrades
                Add(data, "Big Well", ModEntry.Config.EnableWellUpgrade, DataManager.BigWell);

                // Stable upgrades
                AddWithBase(data, "Big Stable", ModEntry.Config.EnableStableUpgrade, "Stable", DataManager.BigStable);
            });
        }
    }
}