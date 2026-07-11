using PipeSystem;
using Verse;

namespace MassProductionExpansion.Generator;

public static class UpgradeGenerator
{
    /// <summary>
    /// Create new processes for upgraded version of building. Recipes should otherwise be the same as previous.
    /// </summary>
    /// <param name="prefix">Text being prepended to the defName</param>
    /// <param name="tickMultiplier">change in work being applied</param>
    /// <param name="defBeingUpgraded">The old buliding</param>
    /// <param name="buildingGettingRecipes">the new building</param>
    /// <param name="hotReload"></param>
    public static ProcessDef UpgradeTier(string prefix, float tickMultiplier,
        ProcessDef defBeingUpgraded, ThingDef buildingGettingRecipes, bool hotReload = false) {
        string defName = prefix + defBeingUpgraded.defName;
        ProcessDef processDef = (hotReload
            ? (DefDatabase<ProcessDef>.GetNamed(defName, errorOnFail: false) ?? new ProcessDef())
            : new ProcessDef());


        processDef.defName = defName;
        processDef.label = defBeingUpgraded.label;
        processDef.labelOverride = defBeingUpgraded.labelOverride;
        processDef.description = defBeingUpgraded.description;
        processDef.priorityInBillList = defBeingUpgraded.priorityInBillList;
        processDef.spawnOnInteractionCell = defBeingUpgraded.spawnOnInteractionCell;
        processDef.autoGrabFromHoppers = defBeingUpgraded.autoGrabFromHoppers;
        processDef.autoInputSlots = defBeingUpgraded.autoInputSlots;
        processDef.disallowMixing = defBeingUpgraded.disallowMixing;
        processDef.ticks = (int)(defBeingUpgraded.ticks * tickMultiplier);
        processDef.ingredients = defBeingUpgraded.ingredients;
        processDef.results = defBeingUpgraded.results;
        processDef.isFactoryProcess = defBeingUpgraded.isFactoryProcess;
        processDef.autoExtract = defBeingUpgraded.autoExtract;
        processDef.onlyGrabAndOutputToFactoryHoppers = defBeingUpgraded.onlyGrabAndOutputToFactoryHoppers;
        processDef.useFirstIngredientAsOutputStuff = defBeingUpgraded.useFirstIngredientAsOutputStuff;
        processDef.hideProcessIfNotNaturalRock = defBeingUpgraded.hideProcessIfNotNaturalRock;
        processDef.rockToDetect=defBeingUpgraded.rockToDetect;
        processDef.sustainerWhenWorking = defBeingUpgraded.sustainerWhenWorking;
        processDef.sustainerDef = defBeingUpgraded.sustainerDef;
        processDef.effecterWhenWorking = defBeingUpgraded.effecterWhenWorking;
        processDef.effecterDef = defBeingUpgraded.effecterDef;
        processDef.maxOutputCount = defBeingUpgraded.maxOutputCount;
        processDef.considerBuildingCompResource = defBeingUpgraded.considerBuildingCompResource;
        processDef.researchPrerequisites = defBeingUpgraded.researchPrerequisites;

        buildingGettingRecipes.GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Add(processDef);
        return processDef;
    }

    public static ProcessDef UpgradeTierCombinedProcesses(string prefix, float tickMultiplier,
        ProcessDef defBeingUpgraded, ThingDef buildingGettingRecipes, int index, bool hotReload = false) {
        string defName = prefix + defBeingUpgraded.defName;
        ProcessDef processDef = (hotReload
            ? (DefDatabase<ProcessDef>.GetNamed(defName, errorOnFail: false) ?? new ProcessDef())
            : new ProcessDef());

        processDef.defName = defName;
        processDef.label = defBeingUpgraded.label;
        processDef.labelOverride = defBeingUpgraded.labelOverride;
        processDef.description = defBeingUpgraded.description;
        processDef.priorityInBillList = index;
        processDef.spawnOnInteractionCell = defBeingUpgraded.spawnOnInteractionCell;
        processDef.autoGrabFromHoppers = defBeingUpgraded.autoGrabFromHoppers;
        processDef.autoInputSlots = defBeingUpgraded.autoInputSlots;
        processDef.disallowMixing = defBeingUpgraded.disallowMixing;
        processDef.ticks = (int)(defBeingUpgraded.ticks * tickMultiplier);
        processDef.ingredients = defBeingUpgraded.ingredients;
        processDef.results = defBeingUpgraded.results;
        processDef.isFactoryProcess = defBeingUpgraded.isFactoryProcess;
        processDef.autoExtract = defBeingUpgraded.autoExtract;
        processDef.onlyGrabAndOutputToFactoryHoppers = defBeingUpgraded.onlyGrabAndOutputToFactoryHoppers;
        processDef.useFirstIngredientAsOutputStuff = defBeingUpgraded.useFirstIngredientAsOutputStuff;
        processDef.hideProcessIfNotNaturalRock = defBeingUpgraded.hideProcessIfNotNaturalRock;
        processDef.rockToDetect=defBeingUpgraded.rockToDetect;
        processDef.sustainerWhenWorking = defBeingUpgraded.sustainerWhenWorking;
        processDef.sustainerDef = defBeingUpgraded.sustainerDef;
        processDef.effecterWhenWorking = defBeingUpgraded.effecterWhenWorking;
        processDef.effecterDef = defBeingUpgraded.effecterDef;
        processDef.maxOutputCount = defBeingUpgraded.maxOutputCount;
        processDef.considerBuildingCompResource = defBeingUpgraded.considerBuildingCompResource;
        processDef.researchPrerequisites = defBeingUpgraded.researchPrerequisites;

        buildingGettingRecipes.GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Add(processDef);
        return processDef;
    }
}