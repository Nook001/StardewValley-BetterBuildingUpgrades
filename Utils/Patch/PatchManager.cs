using HarmonyLib;
using StardewModdingAPI;

namespace BetterBuildingUpgrades;

public static class PatcherManager
{
    private static Harmony? _harmony;

    public static void Init(Mod mod)
    {
        _harmony = new Harmony(mod.ModManifest.UniqueID);
        FineHayPatches.Apply(_harmony);
    }
}