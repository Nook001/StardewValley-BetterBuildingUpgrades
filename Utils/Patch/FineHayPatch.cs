using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;

using System.Linq;
using HarmonyLib;
using SObject = StardewValley.Object;
using xTile.Dimensions;

namespace BetterBuildingUpgrades;

internal static class FineHayPatches
{
    private const int NormalHayId = 178;
    private static string FineHayQualifiedId => "(O)FineHay"; // use Data/Objects key
    private const string AteFineKey = "BetterBuildingUpgrades.AteFineHayToday";
    public static void Apply(Harmony harmony)
    {
        // AnimalHouse.feedAllAnimals: replace placed hay with Fine Hay
        harmony.Patch(
            original: AccessTools.Method(typeof(AnimalHouse), nameof(AnimalHouse.feedAllAnimals)),
            postfix: new HarmonyMethod(typeof(FineHayPatches), nameof(FeedAllAnimals_Postfix))
        );

        // Give Fine Hay when withdrawing from hopper
        var hopperMi = AccessTools.Method(typeof(SObject), "CheckForActionOnFeedHopper");
        if (hopperMi != null)
        {
            harmony.Patch(
                original: hopperMi,
                postfix: new HarmonyMethod(typeof(FineHayPatches), nameof(CheckForActionOnFeedHopper_Postfix))
            );
        }

        // Friendship bonus if trough-fed under Fine mode
        harmony.Patch(
            original: AccessTools.Method(typeof(FarmAnimal), nameof(FarmAnimal.dayUpdate)),
            postfix: new HarmonyMethod(typeof(FineHayPatches), nameof(FarmAnimal_DayUpdate_Postfix))
        );
    }

    // 5.a Replace any placed 178 with Fine Hay right after auto-feed runs
    private static void FeedAllAnimals_Postfix(AnimalHouse __instance)
    {
        if (__instance?.Objects is null) return;

        foreach (var kv in __instance.Objects.Pairs.ToList())
        {
            if (kv.Value is SObject o && !o.bigCraftable.Value && o.ParentSheetIndex == NormalHayId)
            {
                // Replace with Fine Hay object
                __instance.Objects[kv.Key] = ItemRegistry.Create(FineHayQualifiedId) as SObject ;
            }
        }
    }

    // 5.b When withdrawing from hopper, vanilla adds (O)178 to inventory. Convert it to Fine Hay.
    // This avoids messy transpiles and stays compatible.
    private static void CheckForActionOnFeedHopper_Postfix(SObject __instance, Farmer who, bool justCheckingForActivity)
    {
        if (!Context.IsWorldReady || !who.IsLocalPlayer) return;
        if (who.Items is null) return;

        for (int i = 0; i < who.Items.Count; i++)
        {
            if (who.Items[i] is SObject obj && !obj.bigCraftable.Value && obj.ParentSheetIndex == NormalHayId)
            {
                int stack = obj.Stack;
                who.Items[i] = ItemRegistry.Create(FineHayQualifiedId, stack) as SObject
                               ;
            }
        }
    }


    // ---- FarmAnimal.dayUpdate ----
    // Consume Fine Hay before vanilla tries to find (O)178; only host mutates world
    private static void FarmAnimal_DayUpdate_Prefix(FarmAnimal __instance, GameLocation environment)
    {
        if (!Context.IsMainPlayer)
            return;

        try
        {
            if (__instance.fullness.Value >= 200) return;
            if (environment is not AnimalHouse house || house.Objects is null) return;

            // find one Fine Hay in this room
            foreach (var kv in house.Objects.Pairs.ToList())
            {
                if (kv.Value is SObject o && !o.bigCraftable.Value && o.QualifiedItemId == FineHayQualifiedId)
                {
                    // consume it like vanilla would
                    house.Objects.Remove(kv.Key);
                    __instance.fullness.Value = 255;

                    // mark for bonus
                    __instance.modData[AteFineKey] = "1";
                    break;
                }
            }
        }
        catch { /* safety */ }
    }

    // Award extra friendship only if they ate fine hay
    private static void FarmAnimal_DayUpdate_Postfix(FarmAnimal __instance)
    {
        if (!Context.IsMainPlayer)
            return;

        if (__instance.modData.Remove(AteFineKey))
        {
            int bonus = 8; // consider making configurable
            __instance.friendshipTowardFarmer.Value =
                Utility.Clamp(__instance.friendshipTowardFarmer.Value + bonus, 0, 1000);
            // Optional: mood bump
            // __instance.happiness.Value = Utility.Clamp(__instance.happiness.Value + 15, 0, 255);
        }
    }
}