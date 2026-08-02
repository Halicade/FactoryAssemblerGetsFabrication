using System.Collections.Generic;
using System.Linq;
using MassProductionExpansion.Defs;
using PipeSystem;
using RimWorld;
using Verse;

namespace MassProductionExpansion.Implied;

public class ImpliedSmelter
{
    public static IEnumerable<PipeSystem.ProcessDef> ImpliedSmelterProcess(bool hotReload = false) {
        List<ThingDef> smithyRecipes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            !x.Minifiable &&
            (x.costStuffCount != 0 && (x.stuffCategories.NullOrEmpty() ||
                                       x.stuffCategories.Contains(StuffCategoryDefOf.Metallic))
             || x.costList is { Count: <= 4 })
            && (x.recipeMaker?.recipeUsers?.Contains(MPEDefOf.ElectricSmithy) == true ||
                x.recipeMaker?.recipeUsers?.Contains(MPEDefOf.FueledSmithy) == true)
        ).ToList();

        List<ThingDef> recipesForSmelter = smithyRecipes.Where(x =>
            x.costList?.NullOrEmpty() == true
            || x.costList?.Count <= 3
            && x.costList?.ContainsAny(costList => costList.thingDef == ThingDefOf.WoodLog) != true
            || x.stuffCategories?.Contains(StuffCategoryDefOf.Metallic) == true
        ).ToList();


        int smelterCount = MPEDefOf.MPE_HiTechAutomatedSmelter
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;

        foreach (ThingDef def in recipesForSmelter) {
            Log.Message("Creating smelting recipe for " + def);
            yield return Generator.Smelter.ProcessFromSmelterRecipe("MPE_T2Smelter_",
                2,
                MPEDefOf.MPE_Smelter_, def, ++smelterCount,
                MPEDefOf.MPE_HiTechAutomatedSmelter,
                hotReload);
        }

        List<ThingDef> recipesForAssmebler = smithyRecipes.Where(x =>
            x.costList?.Count <= 4
            && x.costList?.ContainsAny(costList => costList.thingDef == ThingDefOf.WoodLog) == true).ToList();


        int machiningBayCount = MPEDefOf.MPE_HiTechAutomatedMachiningBay
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;
        foreach (ThingDef def in recipesForAssmebler) {
            Log.Message("Creating machinig recipe for " + def);
            yield return Generator.Smelter.ProcessFromSmelterRecipe("MPE_T2Machining_",
                2,
                MPEDefOf.MPE_Machining_, def, ++machiningBayCount,
                MPEDefOf.MPE_HiTechAutomatedMachiningBay,
                hotReload);
        }
    }
}