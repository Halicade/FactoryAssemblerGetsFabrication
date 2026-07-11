using System.Collections.Generic;
using System.Linq;
using MassProductionExpansion.Defs;
using MassProductionExpansion.Generator;
using PipeSystem;
using VanillaFurnitureExpandedFactory;
using Verse;
using InternalDefOf = VanillaFurnitureExpandedFactory.InternalDefOf;

namespace MassProductionExpansion.Implied;

public class ImpliedAutoloom
{
    public static IEnumerable<PipeSystem.ProcessDef> ImpliedAutoloomProcesses(bool hotReload = false) {
        List<ThingDef> tailoringBenchRecipes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            //Opposite of VE factorys stuff. We want recipes that have no costStuffCount and do have a costList
            (x.costStuffCount == 0 && x.costList != null)
            //looms only have 2 ports
            && x.costList?.Count < 3
            && x.recipeMaker?.recipeUsers?.Contains(InternalDefOf.HandTailoringBench) == true).ToList();

        MassProductionExpansion.AutoLoomCount = InternalDefOf.VFEFactory_Autoloom
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count + 1;

        foreach (ThingDef def in tailoringBenchRecipes) {
            yield return AutoLoom.ProcessFromTailoringRecipe("MPE_Factory_AutoLoomT1_", 4,
                FactoryDefOf.VFEFactory_Autoloom_, def,
                ++MassProductionExpansion.AutoLoomCount, InternalDefOf.VFEFactory_Autoloom, hotReload);
        }
    }
}