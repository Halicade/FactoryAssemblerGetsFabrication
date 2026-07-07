using System.Collections.Generic;
using System.Linq;
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
            && x.recipeMaker?.recipeUsers?.Contains(InternalDefs.VVE_GarageBench) == true &&
            !x.defName.Contains("Shell")).ToList();

        MassProductionExpansion.AssemblerCount = InternalDefs.VFEFactory_AutomatedAssembler
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;
        foreach (ThingDef def in garageBenchRecipes) {
            yield return Generator.Fabrication.ProcessFromFabricationRecipe("MPE_Factory_Assembler_", 4,
                InternalDefs.VFEFactory_Assembler_, def, ++MassProductionExpansion.FabricatorCount,
                InternalDefs.VFEFactory_AutomatedAssembler,
                hotReload);
        }
    }
}