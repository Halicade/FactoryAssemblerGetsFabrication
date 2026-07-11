using System.Collections.Generic;
using System.Linq;
using MassProductionExpansion;
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
            && x.recipeMaker?.recipeUsers?.Contains(InternalDefOf.DrugLab) == true
        ).ToList();
        MassProductionExpansion.GranulatorCount = FactoryDefOf.VFEFactory_MedicineGranulator
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;
        foreach (ThingDef def in medicines) {
            yield return Medicine.ProcessFromMedicineRecipe("VFEFactory_MedicineGranulatorT1_", 4,
                FactoryDefOf.VFEFactory_Medicine_, def, ++MassProductionExpansion.GranulatorCount,
                FactoryDefOf.VFEFactory_MedicineGranulator,
                hotReload);
        }
    }
}