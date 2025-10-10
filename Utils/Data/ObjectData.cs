using StardewModdingAPI;
using StardewValley.GameData.Objects;
using Microsoft.Xna.Framework;

namespace BetterBuildingUpgrades;

public static partial class DataManager
{
    // Define Fine Hay
    public static ObjectData FineHay(ModConfig config) =>
    new()
    {
        Name = "FineHay",
        DisplayName = ModEntry.Translation.Get("fine-hay.name"),
        Description = ModEntry.Translation.Get("fine-hay.description"),
        Type = "Basic",
        Category = -81,          // category_greens Forage
        Price = 10,

        Texture = "Objects/FineHay",
        Edibility = -300,   // Inedible
        IsDrink = false,
        CanBeGivenAsGift = false,
    };
}