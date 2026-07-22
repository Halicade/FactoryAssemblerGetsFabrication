using System.Collections.Generic;
using System.Linq;
using MassProductionExpansion.Defs;
using PipeSystem;
using Verse;
using Medicine = MassProductionExpansion.Generator.Medicine;

namespace MassProductionExpansion.Implied;

public class ImpliedMedicine
{
    public static IEnumerable<PipeSystem.ProcessDef> ImpliedMedicineProcesses(bool hotReload = false) {
        
        
        List<ThingDef> medicines = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            (x.costStuffCount != 0 || x.costList != null)
            //medical station only has 3 ports
            && x.costList?.Count < 4
            && x.recipeMaker?.recipeUsers?.Contains(MPEDefOf.DrugLab) == true
        ).ToList();
        int granulatorCount = FactoryDefOf.VFEFactory_MedicineGranulator
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;
        foreach (ThingDef def in medicines) {
            yield return Medicine.ProcessFromMedicineRecipe("MPE_GranulatorT2", 2,
                MPEDefOf.MPE_Medicine_, def, ++granulatorCount,
                MPEDefOf.MPE_HiTechMedicineGranulator,
                hotReload);
        }
    }
}