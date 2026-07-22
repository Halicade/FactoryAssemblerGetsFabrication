using System.Collections.Generic;
using System.Linq;
using MassProductionExpansion.Defs;
using PipeSystem;
using Verse;

namespace MassProductionExpansion.Implied;

public class ImpliedStone
{
    public static IEnumerable<PipeSystem.ProcessDef> ImpliedStoneProcesses(bool hotReload = false) {
        // Want to get all natural rocks. Threw some extra logic here just in case
        List<ThingDef> thingsThatDropRocks = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            x.IsNonResourceNaturalRock &&
            x.building?.mineableThing?.deepCommonality == 0 &&
            x.building.mineableDropChance > 0
        ).ToList();

        int assemblerCount = MPEDefOf.MPE_HiTechAutomatedDrillPlatform
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;

        foreach (ThingDef stuffs in thingsThatDropRocks) {
            yield return Generator.Drill.ProcessFromDrillRecipe("MPE_T2Drill_",
                10500,
                stuffs,
                ++assemblerCount,
                MPEDefOf.MPE_HiTechAutomatedDrillPlatform,
                hotReload);
        }
    }
}