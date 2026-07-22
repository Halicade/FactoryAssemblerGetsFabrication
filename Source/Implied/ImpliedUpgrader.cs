using System.Collections.Generic;
using System.Linq;
using MassProductionExpansion.Generator;
using PipeSystem;
using Verse;

namespace MassProductionExpansion.Implied;

public class ImpliedUpgrader
{
    public static int MasonryButcherCounter = 0;
/*
    public static IEnumerable<PipeSystem.ProcessDef> ImpliedMasonryButcherProcess(bool hotReload = false) {
        List<ProcessDef> masonryRecipes = FactoryDefOf.VFEFactory_AutomatedMasonrySaw
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes;

        List<ProcessDef> mincerRecipes = FactoryDefOf.VFEFactory_AutomatedMincer
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes;

        List<ProcessDef> masonryMincerRecipes = [..masonryRecipes, ..mincerRecipes];

        foreach (ProcessDef def in masonryMincerRecipes) {
            
            yield return UpgradeGenerator.UpgradeTierCombinedProcesses(prefix: "MPE_Masonry_T2", tickMultiplier: 0.75f,
                defBeingUpgraded: def,
                buildingGettingRecipes: MPEDefOf.MPE_T2AutomatedMasonrySaw, index: ++MasonryButcherCounter,
                hotReload: hotReload);
        }
    }
*/
    public static IEnumerable<ProcessDef> ImpliedGenericProcess(string prefix, float tickMultiplier, ThingDef defBeingUpgraded,
        ThingDef buildingGettingUpgrades, bool hotReload = false) {
        List<ProcessDef> upgradeableRecipes= defBeingUpgraded.GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes
            .ToList();

        foreach (ProcessDef recipe in upgradeableRecipes) {
            yield return UpgradeGenerator.UpgradeTier(prefix: prefix, tickMultiplier: tickMultiplier,
                defBeingUpgraded: recipe,
                buildingGettingRecipes: buildingGettingUpgrades, hotReload: hotReload);
        }
        
    }
    
}