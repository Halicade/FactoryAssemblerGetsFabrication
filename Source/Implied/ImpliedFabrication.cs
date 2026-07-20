using System.Collections.Generic;
using System.Linq;
using MassProductionExpansion.Defs;
using PipeSystem;
using Verse;

namespace MassProductionExpansion.Implied;

public class ImpliedFabrication
{
    public static IEnumerable<PipeSystem.ProcessDef> AssemblerT2GetsFabricationBench(bool hotReload = false) {
        List<ThingDef> fabricationBenchRecipes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            (x.costStuffCount != 0 || x.costList != null)
            //Assembling station only has 4 ports
            && x.costList?.Count < 5
            //Exclude recipes that have gravlite panels. Could probably have made this a reference to the exact def but oh well
            && !x.defName.Contains("Gravlite")
            && x.recipeMaker?.recipeUsers?.Contains(MPEDefOf
                .FabricationBench) == true &&
            !x.defName.Contains("Shell")).ToList();
        int assemblerCount = MPEDefOf.MPE_HiTechAutomatedAssembler
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;

        foreach (ThingDef def in fabricationBenchRecipes) {
            yield return Generator.Fabrication.ProcessFromFabricationRecipe("MPE_Factory_AssemblerT1_", 
                4,
                MPEDefOf.MPE_Assembler_, def, ++assemblerCount,
                MPEDefOf.MPE_HiTechAutomatedAssembler,
                hotReload);
        }
    }
}