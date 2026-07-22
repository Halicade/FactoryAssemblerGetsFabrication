using MassProductionExpansion.Defs;
using PipeSystem;
using RimWorld;
using Verse;

namespace MassProductionExpansion.Generator;

public class Drill
{
    public static ProcessDef ProcessFromDrillRecipe(string prefix, int ticksToRun,
        ThingDef rockWall, int index, ThingDef defGettingRecipes, bool hotReload = false) {
        ThingDef rockResource = rockWall.building.mineableThing;

        string defName = prefix + rockResource;
        ProcessDef processDef = hotReload
            ? (DefDatabase<ProcessDef>.GetNamed(defName, errorOnFail: false) ?? new ProcessDef())
            : new ProcessDef();
        processDef.defName = defName;
        processDef.label = "MPE_DigToHarvest".Translate(rockResource.label);
        processDef.description = "MPE_DigToHarvest".Translate(rockResource.label);
        processDef.priorityInBillList = index;
        processDef.spawnOnInteractionCell = true;

        processDef.rockToDetect = rockWall;
        processDef.hideProcessIfNotNaturalRock = true;
        processDef.ticks = ticksToRun;
        processDef.results = [
            new ProcessDef.Result {
                thing = rockResource,
                count = 1
            }
        ];

        processDef.isFactoryProcess = true;
        processDef.autoExtract = true;
        processDef.onlyGrabAndOutputToFactoryHoppers = true;
        processDef.sustainerWhenWorking = true;
        processDef.sustainerDef = FactoryDefOf.VFEFactory_AutomatedDrillSustainer;
        processDef.effecterWhenWorking = true;
        processDef.effecterDef = EffecterDefOf.Drill;
        processDef.worksInSpace = false;
        processDef.maxOutputCount = 3;

        defGettingRecipes.GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Add(processDef);
        return processDef;
    }
}