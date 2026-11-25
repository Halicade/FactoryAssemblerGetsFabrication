using System;
using HarmonyLib;
using MassProductionExpansion.Defs;
using PipeSystem;
using RimWorld;
using Verse;

namespace MassProductionExpansion.Patches;

[HarmonyPatch(typeof(Process), nameof(Process.HandleIngredientsAndQuality))]
public static class HandleIngredientsAndQuality
{
    [HarmonyPostfix]
    public static void IncreaseQualityPostFix(Thing outThing, Process __instance) {
        var itemQuality = outThing.TryGetComp<CompQuality>();
        if (itemQuality == null) {
            return;
        }

        CompAffectedByFacilities linkables =
            __instance.advancedProcessor.parent.TryGetComp<CompAffectedByFacilities>();

        if (linkables == null) {
            return;
        }

        bool itemCanBeUpgraded = false;
        foreach (var linkedThings in linkables.LinkedFacilitiesListForReading) {
            if (linkedThings.def == MPEDefOf.MPE_MechUplink) {
                itemCanBeUpgraded = true;
                break;
            }
        }

        if (!itemCanBeUpgraded) {
            return;
        }

        if (!outThing.TryGetQuality(out QualityCategory qualityCategory)) {
            return;
        }

        int currentQuality = (int)qualityCategory;
        if (Rand.Bool) {
            currentQuality++;
            if (Rand.Bool) {
                currentQuality++;
            }
        }

        currentQuality = Math.Min(currentQuality, 6);
        itemQuality.SetQuality((QualityCategory)currentQuality, null);
    }
}