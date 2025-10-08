// Utils/AssetManager.Buildings.cs
using StardewModdingAPI;
using StardewModdingAPI.Events;

namespace BetterBuildingUpgrades;

public static partial class ContentManager
{
    private static readonly Dictionary<string, string> TextureMappings = new()
    {
        { "Buildings/Big Silo", "assets/Big_Silo.png" },
        { "Buildings/Deluxe Silo", "assets/Deluxe_Silo.png" },
        { "Buildings/Grinding Silo", "assets/Grinding_Silo.png" },
        { "Buildings/Big Greenhouse", "assets/Big_Greenhouse.png" },
        { "Buildings/Deluxe Greenhouse", "assets/Deluxe_Greenhouse.png" },
        { "Buildings/Big Well", "assets/Big_Well.png" },
        { "Buildings/Big Stable", "assets/Big_Stable.png" },

        { "Objects/FineHay", "assets/Fine_Hay.png" },
    };

    // Load the custom textures for the buildings
    public static void LoadTextures(object? sender, AssetRequestedEventArgs e)
    {
        if (TextureMappings.TryGetValue(e.NameWithoutLocale.BaseName, out var filePath))
        {
            e.LoadFromModFile<Microsoft.Xna.Framework.Graphics.Texture2D>(filePath, AssetLoadPriority.Medium);
        }
    }
}