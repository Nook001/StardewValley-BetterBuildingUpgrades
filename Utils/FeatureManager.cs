using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Buffs;
using Microsoft.Xna.Framework;

namespace BetterBuildingUpgrades;

/// <summary>
/// Handles game features and functionality
/// </summary>
public static class FeatureManager
{
    /// <summary>
    /// Call on day start
    /// </summary>
    public static void OnDayStarted(object? sender, DayStartedEventArgs e)
    {
        // Remove the original greenhouse if Big Greenhouse exists
        RefreshGreenhouseMap();

        // Accelerate crops in deluxe greenhouse
        AccelerateDeluxeGreenhouseCrops(sender, e);
    }
    
    private static void RefreshGreenhouseMap()
    {
        ModEntry.ModHelper.GameContent.InvalidateCache("Maps/Greenhouse");

        // Force Greenhouse reloading
        foreach (GameLocation location in Game1.locations)
        {
            if (location.Name == "Greenhouse") location.loadMap("Maps/Greenhouse", true); 
        }
    }
    
    private static void AccelerateDeluxeGreenhouseCrops(object? sender, DayStartedEventArgs e)
    {
        // Skip if feature is disabled or no upgraded greenhouse
        if (!BuildingDetector.HasDeluxeGreenhouse())
            return;
            
        // Get the greenhouse location
        GameLocation greenhouse = Game1.getLocationFromName("Greenhouse");
        if (greenhouse is null)
            return;
            
        // Process all tiles in the greenhouse
        foreach (var tile in greenhouse.terrainFeatures.Pairs)
        {
            // Check if the terrain feature is a crop
            if (tile.Value is not StardewValley.TerrainFeatures.HoeDirt { crop: not null } dirt)
			    continue;
            
            var crop = dirt.crop;

            // Skip fully grown or dead crops
            if (crop.fullyGrown.Value || crop.dead.Value)
                continue;

            // Only accelerate newly planted crops
            if (crop.dayOfCurrentPhase.Value + crop.phaseDays.Take(crop.currentPhase.Value).Sum() > 1)
                continue;

            // Calculate the days to accelerate
            int daysToAccelerate = (int) Math.Floor(0.3 * (crop.phaseDays.Sum() - 99999));
            // Apply the acceleration
            for (int i = 0; i < crop.phaseDays.Count - 1; i++)
            {
                if (crop.phaseDays[i] < daysToAccelerate)
                {
                    daysToAccelerate -= crop.phaseDays[i];
                }
                else
                {
                    crop.dayOfCurrentPhase.Value = daysToAccelerate + 1;
                    crop.currentPhase.Value = i;
                    break;
                }
            }
        }
    }
    

    /// <summary>
    /// Call when the game save is loaded
    /// </summary>
    public static void OnSaveLoaded(object? sender, SaveLoadedEventArgs e)
    {
        // Remove the original greenhouse if Big Greenhouse exists
        ModEntry.ModHelper.Events.GameLoop.OneSecondUpdateTicked += CleanupDuplicateGreenhouse;
    }
    
    public static void CleanupDuplicateGreenhouse(object? sender, OneSecondUpdateTickedEventArgs e)
    {      
        ModEntry.ModHelper.Events.GameLoop.OneSecondUpdateTicked -= CleanupDuplicateGreenhouse;
        if (!Context.IsWorldReady || !BuildingDetector.HasUpgradedGreenhouse())
            return;

        // Remove the original greenhouse if Big Greenhouse exists
        Farm farm = Game1.getFarm();
        foreach (var building in farm.buildings)
        {
            if (building.buildingType.Value == "Greenhouse" && BuildingDetector.HasUpgradedGreenhouse())
            {
                farm.buildings.Remove(building);
                break;
            }
            
            if (building.buildingType.Value == "Big Greenhouse" && BuildingDetector.HasDeluxeGreenhouse())
            {
                farm.buildings.Remove(building);
                break;
            }
        }
    }
    


    // Fix player warp for modding map
    public static void OnPlayerWarped(object? sender, WarpedEventArgs e)
    {
        // If player is entering upgraded greenhouse
        if (e.NewLocation.Name == "Greenhouse" && BuildingDetector.HasUpgradedGreenhouse())
        {
            ModEntry.ModHelper.Events.GameLoop.UpdateTicked += UpdateGreenhouseWarp;
        }
    }
    

    // Warp player to corret entrance of the greenhouse
    private static void UpdateGreenhouseWarp(object? sender, UpdateTickedEventArgs e)
    {
        ModEntry.ModHelper.Events.GameLoop.UpdateTicked -= UpdateGreenhouseWarp; // Unsubscribe from the event
        if (Game1.getLocationFromName("Greenhouse") is null) { return; }
        
        Game1.player.Position = ModEntry.Config.FrontierFarmCompatibilityMode 
            ? new Vector2(13*64, 31*64)  // Frontier Farm
            : new Vector2(12*64, 28*64); // Original Greenhouse
    }
    

    public static void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        // Check if the world and player are ready
        if (!Context.IsWorldReady || Game1.player is null)
            return;

        // Player is on the horse and has a big stable
        if (Game1.player.mount is not null && BuildingDetector.HasBigStable())
            AccelerateHorseSpeed();
    }

    private static void AccelerateHorseSpeed()
    {    
        Game1.player.applyBuff(new Buff(
            id: "HorseBuff",
            duration: 20,
            effects: new BuffEffects() { Speed = { 2 } }
        ));
    }
}