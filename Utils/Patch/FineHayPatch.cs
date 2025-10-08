using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using StardewValley;
using StardewValley.Locations;
using SObject = StardewValley.Object;
using StardewModdingAPI;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;

namespace BetterBuildingUpgrades;

internal static class FineHayPatches
{
    private const int NormalHayId = 178;
    private const string FineHayQualifiedId = "(O)FineHay"; // must match your Data/Objects key
    private const string AteFineKey = "BetterBuildingUpgrades.AteFineHayToday";

    private static bool IsFineMode() => BuildingDetector.HasGrindingSilo();

    // Track per-call which tiles already had hay before auto-feed
    private static readonly ConditionalWeakTable<AnimalHouse, HashSet<Vector2>> PreFeedHayTiles = new();

    public static void Apply(Harmony harmony)
    {
        // Auto-feed: snapshot pre-existing hay tiles, then replace only newly placed 178 with Fine Hay
        harmony.Patch(
            original: AccessTools.Method(typeof(AnimalHouse), nameof(AnimalHouse.feedAllAnimals)),
            prefix: new HarmonyMethod(typeof(FineHayPatches), nameof(FeedAllAnimals_Prefix)),
            postfix: new HarmonyMethod(typeof(FineHayPatches), nameof(FeedAllAnimals_Postfix))
        );

        // Hopper/Silo: convert withdrawn normal hay to Fine Hay in inventory
        var miHopper = AccessTools.Method(typeof(SObject), "CheckForActionOnFeedHopper");
        if (miHopper != null)
        {
            harmony.Patch(
                original: miHopper,
                postfix: new HarmonyMethod(typeof(FineHayPatches), nameof(CheckForActionOnFeedHopper_Postfix))
            );
        }

        // Animals: consume Fine Hay before vanilla (which only eats (O)178), then bonus after
        harmony.Patch(
            original: AccessTools.Method(typeof(FarmAnimal), nameof(FarmAnimal.dayUpdate)),
            prefix: new HarmonyMethod(typeof(FineHayPatches), nameof(FarmAnimal_DayUpdate_Prefix)),
            postfix: new HarmonyMethod(typeof(FineHayPatches), nameof(FarmAnimal_DayUpdate_Postfix))
        );
    }

    // ---- AnimalHouse.feedAllAnimals ----
    private static void FeedAllAnimals_Prefix(AnimalHouse __instance)
    {
        if (!Context.IsMainPlayer || !IsFineMode() || __instance?.Objects is null)
            return;

        var pre = new HashSet<Vector2>();
        foreach (var kv in __instance.Objects.Pairs)
        {
            if (kv.Value is SObject o && !o.bigCraftable.Value && o.ParentSheetIndex == NormalHayId)
                pre.Add(kv.Key);
        }
        // store snapshot for this call
        PreFeedHayTiles.Remove(__instance);
        PreFeedHayTiles.Add(__instance, pre);
    }

    private static void FeedAllAnimals_Postfix(AnimalHouse __instance)
    {
        if (!Context.IsMainPlayer || !IsFineMode() || __instance?.Objects is null)
            return;

        // get snapshot
        HashSet<Vector2>? pre;
        if (!PreFeedHayTiles.TryGetValue(__instance, out pre))
            pre = new HashSet<Vector2>();

        // replace only newly placed normal hay
        foreach (var kv in __instance.Objects.Pairs.ToList())
        {
            if (kv.Value is SObject o && !o.bigCraftable.Value && o.ParentSheetIndex == NormalHayId)
            {
                if (!pre.Contains(kv.Key))
                {
                    // This (O)178 was placed by auto-feed now; replace with Fine Hay
                    __instance.Objects[kv.Key] = ItemRegistry.Create(FineHayQualifiedId) as SObject;
                }
            }
        }

        // cleanup snapshot
        PreFeedHayTiles.Remove(__instance);
    }

    // ---- SObject.CheckForActionOnFeedHopper (withdraw hay) ----
    private static void CheckForActionOnFeedHopper_Postfix(SObject __instance, Farmer who, bool justCheckingForActivity)
    {
        if (!Context.IsWorldReady || !who.IsLocalPlayer || !IsFineMode())
            return;

        // convert any normal hay that was just added to inventory
        if (who.Items is null) return;
        for (int i = 0; i < who.Items.Count; i++)
        {
            if (who.Items[i] is SObject o && !o.bigCraftable.Value && o.ParentSheetIndex == NormalHayId)
            {
                int stack = o.Stack;
                who.Items[i] = ItemRegistry.Create(FineHayQualifiedId, stack) as SObject
                                        ;
            }
        }
    }

    // ---- FarmAnimal.dayUpdate ----
    // Consume Fine Hay before vanilla tries to find (O)178; only host mutates world
    private static void FarmAnimal_DayUpdate_Prefix(FarmAnimal __instance, GameLocation environment)
    {
        if (!Context.IsMainPlayer || !IsFineMode())
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
        catch {}
    }

    // Award extra friendship only if they ate fine hay
    private static void FarmAnimal_DayUpdate_Postfix(FarmAnimal __instance)
    {
        if (!Context.IsMainPlayer || !IsFineMode())
            return;

        if (__instance.modData.Remove(AteFineKey))
        {
            int bonus = 80; // consider making configurable
            __instance.friendshipTowardFarmer.Value =
                Utility.Clamp(__instance.friendshipTowardFarmer.Value + bonus, 0, 1000);
            // Optional: mood bump
            // __instance.happiness.Value = Utility.Clamp(__instance.happiness.Value + 15, 0, 255);
        }
    }
}