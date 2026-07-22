using System.Collections.Generic;
using System.Linq;
using MassProductionExpansion.Defs;
using PipeSystem;
using Verse;

namespace MassProductionExpansion.Implied;

public class ImpliedVehicles
{
    public static IEnumerable<PipeSystem.ProcessDef> ImpliedGarageProcesses(bool hotReload = false) {
        List<ThingDef> garageBenchRecipes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            (x.costStuffCount != 0 || x.costList != null)
            //Assembling station only has 4 ports
            && x.costList?.Count < 5
            && x.recipeMaker?.recipeUsers?.Contains(MPEDefOf.VVE_GarageBench) == true &&
            !x.defName.Contains("Shell")).ToList();

        int assemblerCount = MPEDefOf.MPE_HiTechAutomatedAssembler
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;
        
        foreach (ThingDef def in garageBenchRecipes) {
            yield return Generator.Fabrication.ProcessFromFabricationRecipe("MPE_Factory_Assembler_", 4,
                MPEDefOf.MPE_Assembler_, def, ++assemblerCount,
                MPEDefOf.MPE_HiTechAutomatedAssembler,
                hotReload);
        }
    }
}