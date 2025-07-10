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
        // Check if the world is ready and the player is not null

        // Remove the original greenhouse if Big Greenhouse exists
        RefreshGreenhouseMap();
        AccelerateDeluxeGreenhouseCrops(sender, e);
    }
    
    private static void RefreshGreenhouseMap()
    {
        ModEntry.ModHelper.GameContent.InvalidateCache("Maps/Greenhouse");

        // Force all locations to reload
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
        if (greenhouse == null)
            return;
            
        // Process all crops in the greenhouse
        foreach (var tile in greenhouse.terrainFeatures.Pairs)
        {
            // Check if the terrain feature is a crop
            if (tile.Value is StardewValley.TerrainFeatures.HoeDirt dirt && dirt.crop != null)
            {
                var crop = dirt.crop;

                // Skip fully grown or dead crops or crops that are not in the first day of growth
                if (crop.fullyGrown.Value || crop.dead.Value)
                    continue;

                // Only accelerate crops that are newly planted (within first 2 days)
                int totalDaysGrown = crop.dayOfCurrentPhase.Value;
                for (int i = 0; i < crop.currentPhase.Value; i++)
                {
                    totalDaysGrown += crop.phaseDays[i];
                }
                
                if (totalDaysGrown > 1)
                    continue;

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
    


    /// <summary>
    /// Call when the player warps to a new location
    /// </summary>
    public static void OnPlayerWarped(object? sender, WarpedEventArgs e)
    {
        // If player is entering upgraded greenhouse
        if (e.NewLocation.Name == "Greenhouse" && BuildingDetector.HasUpgradedGreenhouse())
        {
            ModEntry.ModHelper.Events.GameLoop.UpdateTicked += UpdateGreenhouseWarp;
        }
    }
    
    private static void UpdateGreenhouseWarp(object? sender, UpdateTickedEventArgs e)
    {
        ModEntry.ModHelper.Events.GameLoop.UpdateTicked -= UpdateGreenhouseWarp; // Unsubscribe from the event
        if (Game1.getLocationFromName("Greenhouse") != null)
        {
            if (ModEntry.Config.FrontierFarmCompatibilityMode)
            {
                Game1.player.Position = new Vector2(13*64, 31*64); // Frontier Farm
            }
            else
            {
                Game1.player.Position = new Vector2(12*64, 28*64); // Original Greenhouse
            }
        }   
    }
    


    /// <summary>
    /// Call every tick
    /// </summary>
    public static void OnUpdateTicked(object? sender, UpdateTickedEventArgs e)
    {
        if (!Context.IsWorldReady || Game1.player == null)
            return;

        // Player is on the horse and has a big stable
        if (Game1.player.mount != null && BuildingDetector.HasBigStable())
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