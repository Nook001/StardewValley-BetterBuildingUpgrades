using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley.GameData.Buildings;
using StardewValley.GameData.Objects;

namespace BetterBuildingUpgrades;

public static partial class ContentManager
{
    public static void LoadItems(object? sender, AssetRequestedEventArgs e)
    {
        if (e.NameWithoutLocale.IsEquivalentTo("Data/Objects"))
        {
            e.Edit(asset =>
            {
                var data = asset.AsDictionary<string, ObjectData>().Data;

                data["FineHay"] = DataManager.FineHay(ModEntry.Config);
            });
        }
    }
}