using StardewModdingAPI;
using StardewValley.GameData.Buildings;
using Microsoft.Xna.Framework;

namespace BetterBuildingUpgrades;

public static partial class DataManager
{
    const bool Debug = false;

    // Silo
    public static BuildingData BigSilo (ModConfig config) => 
    new()
    {   
        // Required fields
        Name = ModEntry.Translation.Get("big-silo.name"),
        Description = ModEntry.Translation.Get("big-silo.description"),
        Texture = config.RetextureCompatibilityMode ? "Buildings/Silo" : "Buildings/Big Silo",
        // Construction
        Builder = "Robin",
        BuildCost = 500,
        BuildDays = Debug ? 0 : 1,
        BuildMaterials = Debug ? new List<BuildingMaterial>{} :
        new List<BuildingMaterial>
        {
            new() { ItemId = "(O)390", Amount = 200 }, // Stone
            new() { ItemId = "(O)330", Amount = 10 }, // Clay
            new() { ItemId = "(O)334", Amount = 5 }, // Copper Bar                        
        },
        BuildingToUpgrade = "Silo", // Base Building
        Size = new Point(3, 3),
        HayCapacity = 500,
        DefaultAction = "BuildingSilo",
    };

    public static BuildingData DeluxeSilo (ModConfig config) =>
    new()
    {   
        // Description
        Name = ModEntry.Translation.Get("deluxe-silo.name"),
        Description = ModEntry.Translation.Get("deluxe-silo.description"),
        Texture = config.RetextureCompatibilityMode ? "Buildings/Silo" : "Buildings/Deluxe Silo",
        // Construction data
        Builder = "Robin",
        BuildCost = 1500,
        BuildDays = Debug ? 0 : 1,
        BuildMaterials = Debug ? new List<BuildingMaterial>{} :
        new List<BuildingMaterial>
        {
            new() { ItemId = "(O)390", Amount = 350 }, // Stone
            new() { ItemId = "(O)330", Amount = 15 }, // Clay
            new() { ItemId = "(O)334", Amount = 20 }, // Copper Bar                        
        },
        BuildingToUpgrade = "Big Silo", // Base Building
        Size = new Point(3, 3),
        HayCapacity = 1000,
        DefaultAction = "BuildingSilo",
    };


    public static BuildingData GrindingSilo (ModConfig config) =>
    new()
    {
        // Description
        Name = ModEntry.Translation.Get("grinding-silo.name"),
        Description = ModEntry.Translation.Get("grinding-silo.description"),
        Texture = config.RetextureCompatibilityMode ? "Buildings/Silo" : "Buildings/Grinding Silo",
        // Construction data
        Builder = "Robin",
        BuildCost = 2500,
        BuildDays = Debug ? 0 : 2,
        BuildMaterials = Debug ? new List<BuildingMaterial>{} :
        new List<BuildingMaterial>
        {
            new() { ItemId = "(O)390", Amount = 400 }, // Stone
            new() { ItemId = "(O)335", Amount = 30 }, // Iron Bar
            new() { ItemId = "(O)334", Amount = 20 }, // Copper Bar                        
        },

        // Upgrade Path
        BuildingToUpgrade = "Deluxe Silo", // Base Building

        // Silo Behavior
        Size = new Point(3, 3),
        HayCapacity = 1200,
        DefaultAction = "BuildingSilo",
    };




    // Greenhouse
    public static BuildingData BigGreenHouse (ModConfig config) =>
    new()
    {   
        // Description
        Name = ModEntry.Translation.Get("big-greenhouse.name"),
        Description = ModEntry.Translation.Get("big-greenhouse.description"),
        Texture = config.RetextureCompatibilityMode ? "Buildings/Greenhouse" :"Buildings/Big Greenhouse",
        SourceRect = config.RetextureCompatibilityMode ? new Rectangle(0, 160, 112, 160) : new Rectangle(0, 0, 112, 160),

        // Construction data
        Builder = "Robin",
        BuildCost = 50000,
        BuildDays = Debug ? 1 : 3,
        AdditionalPlacementTiles = new List<BuildingPlacementTile>{new() { TileArea = new Rectangle(2, 6, 3, 3) },},
        BuildMaterials = Debug ? new List<BuildingMaterial>{} :
        new List<BuildingMaterial>
        {
            new() { ItemId = "(O)388", Amount = 500 }, // Wood
            new() { ItemId = "(O)709", Amount = 50 }, // Hardwood
            new() { ItemId = "(O)334", Amount = 20 }, // Copper Bar  
        },
        BuildingToUpgrade = "Greenhouse",
        IndoorMap = "Greenhouse",
        NonInstancedIndoorLocation = "Greenhouse",
        Size = new Point(7, 6),
        HumanDoor = new Point(3, 5),
    };

    public static BuildingData DeluxeGreenHouse (ModConfig config) =>
    new()
    {   
        // Description
        Name = ModEntry.Translation.Get("deluxe-greenhouse.name"),
        Description = ModEntry.Translation.Get("deluxe-greenhouse.description"),
        Texture = config.RetextureCompatibilityMode ? "Buildings/Greenhouse" :"Buildings/Deluxe Greenhouse",
        SourceRect = config.RetextureCompatibilityMode ? new Rectangle(0, 160, 112, 160) : new Rectangle(0, 0, 112, 160),

        // Construction data
        Builder = "Robin",
        BuildCost = 150000,
        BuildDays = Debug ? 1 : 3,
        AdditionalPlacementTiles = new List<BuildingPlacementTile>{new() { TileArea = new Rectangle(2, 6, 3, 3) },},
        BuildMaterials = Debug ? new List<BuildingMaterial>{} :
        new List<BuildingMaterial>
        {
            new() { ItemId = "(O)787", Amount = 30 }, // Battery Pack
            new() { ItemId = "(O)709", Amount = 100 }, // Hardwood
            new() { ItemId = "(O)337", Amount = 20 }, // Iridium Bar  
        },
        BuildingToUpgrade = "Big Greenhouse",
        IndoorMap = "Deluxe Greenhouse",
        NonInstancedIndoorLocation = "Greenhouse",
        Size = new Point(7, 6),
        HumanDoor = new Point(3, 5),
    };



    // Well
    public static BuildingData BigWell (ModConfig config) =>
    new()
    {
        // Description
        Name = ModEntry.Translation.Get("big-well.name"),
        Description = ModEntry.Translation.Get("big-well.description"),
        Texture = config.RetextureCompatibilityMode ? "Buildings/Well" : "Buildings/Big Well",

        // Construction data
        Builder = "Robin",
        BuildCost = 1500,
        BuildDays = Debug ? 0 : 1,
        BuildMaterials = Debug ? new List<BuildingMaterial>{} :
        new List<BuildingMaterial>
        {
            new() { ItemId = "(O)390", Amount = 200 }, // Stone
        },
        BuildingToUpgrade = "Well",
        Size = new Point(3, 3),
        TileProperties = new List<BuildingTileProperty>
        {
            new(){ Id = "CenterWater", Name = "Water", Value = "T", Layer = "Back", TileArea = new Rectangle(0, 0, 3, 3)}
        },
    };


    // Stable
    public static BuildingData BigStable (ModConfig config, BuildingData stable) =>
    new()
    {
        // Description
        Name = ModEntry.Translation.Get("big-stable.name"),
        Description = ModEntry.Translation.Get("big-stable.description"),
        SortTileOffset = stable.SortTileOffset,
        DrawLayers = stable.DrawLayers,
        Texture = config.RetextureCompatibilityMode ? "Buildings/Stable" : "Buildings/Big Stable",

        // Construction data
        Builder = "Robin",
        BuildCost = 10000,
        BuildDays = Debug ? 0 : 1,
        BuildMaterials = Debug ? new List<BuildingMaterial>{} :
        new List<BuildingMaterial>
        {
            new() { ItemId = "(O)709", Amount = 30 }, // Hardwood
            new() { ItemId = "(O)335", Amount = 5 }, // Copper Bar  
        },
        BuildingToUpgrade = "Stable",
        Size = new Point(4, 2),
        CollisionMap = "XXXX\nXOOX",
    };


    // Fish Pond鱼菜共生系统
    public static BuildingData BigFishPond (ModConfig config) =>
    new()
    {

    };

    // 磨坊
    // 雨天加速，更多产品
}