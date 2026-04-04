using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using HarmonyLib;
using PipeSystem;
using VanillaFurnitureExpandedFactory;
using Verse;

namespace MassProductionExpansion;

public class FactoryAssemblerGetsFabrication : Mod
{
    public FactoryAssemblerGetsFabrication(ModContentPack content) : base(content) {
        Harmony harmony = new("Hali.FactoryAssemblerGetsFabrication");

        Type patchType = typeof(FactoryAssemblerGetsFabrication);

        harmony.Patch(AccessTools.Method(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve"),
            prefix: new HarmonyMethod(patchType, nameof(FactoryStuffToAssemblerPostFix)));
    }

    private static int _autoLoomCount = 0;
    private static int _fabricatorCount = 0;
    private static int _assemblerCount = 0;
    private static int _granulatorCount = 0;

    private static bool someModIsActive;

    //Loading this after VE factory cause it seems like a smart idea
    [HarmonyAfter("com.VanillaFurnitureExpandedFactory")]
    public static void FactoryStuffToAssemblerPostFix(bool hotReload = false) {


        someModIsActive = ModLister.GetActiveModWithIdentifier("oskarpotocki.vanillavehiclesexpanded") != null;
        
        
        foreach (PipeSystem.ProcessDef item in ImpliedAutoloomProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }


        foreach (PipeSystem.ProcessDef item in ImpliedFabricationProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedMedicineProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        if (ModLister.GetActiveModWithIdentifier("oskarpotocki.vanillavehiclesexpanded") != null) {
            foreach (PipeSystem.ProcessDef item in ImpliedGarageProcesses(hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        if (ModLister.GetActiveModWithIdentifier("FrozenSnowFox.FrozenSnowFoxTweaks") != null) {
            foreach (PipeSystem.ProcessDef item in ImpliedFSFBionicProcesses(hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }
/*
        InternalDefs.MyNewMachine.GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes
            = InternalDefOf.VFEFactory_AutomatedMachiningBay
                .GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes;
                */
    }

    public static IEnumerable<PipeSystem.ProcessDef> ImpliedAutoloomProcesses(bool hotReload = false) {
        List<ThingDef> tailoringBenchRecipes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            //Opposite of VE factorys stuff. We want recipes that have no costStuffCOunt and do have a costList
            (x.costStuffCount == 0 && x.costList != null)

            //looms only have 2 ports
            && x.costList?.Count < 3
            && x.recipeMaker?.recipeUsers?.Contains(InternalDefOf.HandTailoringBench) == true).ToList();

        _autoLoomCount = InternalDefOf.VFEFactory_Autoloom.GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count + 1;

        foreach (ThingDef def in tailoringBenchRecipes) {
            foreach (AutoloomProcessTemplateDef templateDef in DefDatabase<AutoloomProcessTemplateDef>.AllDefs) {
                yield return ProcessFromTailoringRecipe(templateDef, def, ++_autoLoomCount, hotReload);
            }
        }
    }


    public static ProcessDef ProcessFromTailoringRecipe(AutoloomProcessTemplateDef tp, ThingDef def, int index,
        bool hotReload = false) {
        string defName = tp.defName + def.defName;
        ProcessDef processDef = (hotReload
            ? (DefDatabase<ProcessDef>.GetNamed(defName, errorOnFail: false) ?? new ProcessDef())
            : new ProcessDef());
        processDef.defName = defName;
        processDef.label = tp.label.Formatted(def.label);
        processDef.description = tp.description.Formatted(def.label);
        processDef.priorityInBillList = index;
        processDef.spawnOnInteractionCell = tp.spawnOnInteractionCell;
        processDef.autoGrabFromHoppers = tp.autoGrabFromHoppers;
        processDef.autoInputSlots = tp.autoInputSlots;
        processDef.disallowMixing = tp.disallowMixing;

        float calculatedTicks = def.GetStatValueAbstract(StatDefOf.WorkToMake);
        if (calculatedTicks <= 0) {
            calculatedTicks = 3200;
        }

        processDef.ticks = (int)(calculatedTicks * 4);

        if (def.stuffCategories?.Contains(StuffCategoryDefOf.Metallic) == true) {
            processDef.ingredients.Add(new ProcessDef.Ingredient {
                thing = ThingDefOf.Steel,
                countNeeded = def.costStuffCount
            });
        }

        if (def.costList != null) {
            List<ProcessDef.Ingredient> ingredientList = new List<ProcessDef.Ingredient>();
            foreach (var cost in def.costList) {
                ingredientList.Add(new ProcessDef.Ingredient {
                    thing = cost.thingDef,
                    countNeeded = cost.count
                });
            }

            processDef.ingredients.AddRange(ingredientList);
        }

        ThingCategoryDef category = null;
        if (def.stuffCategories?.Contains(StuffCategoryDefOf.Fabric) == true) {
            category = ThingCategoryDefOf.Textiles;
        }

        if (category != null) {
            processDef.ingredients.Add(new ProcessDef.Ingredient {
                thingCategory = category,
                countNeeded = def.costStuffCount
            });
        }

        processDef.results = new List<ProcessDef.Result> {
            new ProcessDef.Result {
                thing = def,
                count = 1
            }
        };
        processDef.isFactoryProcess = tp.isFactoryProcess;
        processDef.autoExtract = tp.autoExtract;
        processDef.onlyGrabAndOutputToFactoryHoppers = tp.onlyGrabAndOutputToFactoryHoppers;
        processDef.useFirstIngredientAsOutputStuff = tp.useFirstIngredientAsOutputStuff;
        processDef.sustainerWhenWorking = tp.sustainerWhenWorking;
        processDef.sustainerDef = tp.sustainerDef;
        processDef.effecterWhenWorking = tp.effecterWhenWorking;
        processDef.effecterDef = tp.effecterDef;
        processDef.maxOutputCount = tp.maxOutputCount;
        if (def.recipeMaker?.researchPrerequisites != null) {
            processDef.researchPrerequisites = new List<ResearchProjectDef>();
            processDef.researchPrerequisites.AddRange(def.recipeMaker.researchPrerequisites);
        }
        else if (def.recipeMaker?.researchPrerequisite != null) {
            processDef.researchPrerequisites = new List<ResearchProjectDef>();
            processDef.researchPrerequisites.Add(def.recipeMaker.researchPrerequisite);
        }

        //This is the part that adds the recipe
        InternalDefOf.VFEFactory_Autoloom.GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes
            .Add(processDef);
        return processDef;
    }


    public static IEnumerable<PipeSystem.ProcessDef> ImpliedFabricationProcesses(bool hotReload = false) {
        List<ThingDef> fabricationBenchRecipes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            (x.costStuffCount != 0 || x.costList != null)
            //Assembling station only has 4 ports
            && x.costList?.Count < 5
            //Exclude recipes that have gravlite panels. Could probably have made this a reference to the exact def but oh well
            && !x.defName.Contains("Gravlite")
            && x.recipeMaker?.recipeUsers?.Contains(InternalDefs
                .FabricationBench) == true &&
            !x.defName.Contains("Shell")).ToList();
        _assemblerCount = InternalDefs.VFEFactory_AutomatedAssembler
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;

        foreach (ThingDef def in fabricationBenchRecipes) {
            yield return ProcessFromFabricationRecipe(InternalDefs.VFEFactory_Assembler_, def, ++_assemblerCount,
                hotReload);
        }
    }

    public static ProcessDef ProcessFromFabricationRecipe(ProcessorTemplateDef tp, ThingDef def, int index,
        bool hotReload = false) {
        string defName = "VFEFactory_Assembler_" + def.defName;
        ProcessDef processDef = (hotReload
            ? (DefDatabase<ProcessDef>.GetNamed(defName, errorOnFail: false) ?? new ProcessDef())
            : new ProcessDef());
        processDef.defName = defName;
        processDef.label = tp.label.Formatted(def.label);
        processDef.description = tp.description.Formatted(def.label);
        processDef.priorityInBillList = index;
        processDef.spawnOnInteractionCell = tp.spawnOnInteractionCell;
        processDef.autoGrabFromHoppers = tp.autoGrabFromHoppers;
        processDef.autoInputSlots = tp.autoInputSlots;
        processDef.disallowMixing = tp.disallowMixing;

        float calculatedTicks = def.GetStatValueAbstract(StatDefOf.WorkToMake);
        if (calculatedTicks <= 0) {
            calculatedTicks = 3200;
        }

        processDef.ticks = (int)(calculatedTicks * 4);


        if (def.stuffCategories?.Contains(StuffCategoryDefOf.Metallic) == true) {
            processDef.ingredients.Add(new ProcessDef.Ingredient {
                thing = ThingDefOf.Steel,
                countNeeded = def.costStuffCount
            });
        }

        if (def.costList != null) {
            List<ProcessDef.Ingredient> ingredientList = new List<ProcessDef.Ingredient>();
            foreach (var cost in def.costList) {
                ingredientList.Add(new ProcessDef.Ingredient {
                    thing = cost.thingDef,
                    countNeeded = cost.count
                });
            }

            processDef.ingredients.AddRange(ingredientList);
        }

        ThingCategoryDef category = null;
        if (def.stuffCategories?.Contains(StuffCategoryDefOf.Fabric) == true) {
            category = ThingCategoryDefOf.Textiles;
        }

        if (category != null) {
            processDef.ingredients.Add(new ProcessDef.Ingredient {
                thingCategory = category,
                countNeeded = def.costStuffCount
            });
        }

        processDef.results = new List<ProcessDef.Result> {
            new ProcessDef.Result {
                thing = def,
                count = 1
            }
        };
        processDef.isFactoryProcess = tp.isFactoryProcess;
        processDef.autoExtract = tp.autoExtract;
        processDef.onlyGrabAndOutputToFactoryHoppers = tp.onlyGrabAndOutputToFactoryHoppers;
        processDef.useFirstIngredientAsOutputStuff = def.costStuffCount != 0;
        processDef.sustainerWhenWorking = tp.sustainerWhenWorking;
        processDef.sustainerDef = tp.sustainerDef;
        processDef.effecterWhenWorking = tp.effecterWhenWorking;
        processDef.effecterDef = tp.effecterDef;
        processDef.maxOutputCount = tp.maxOutputCount;

        if (ModLister.HasActiveModWithName("Vanilla Chemfuel Expanded") &&
            processDef.ingredients.ContainsAny(x => x.thing == ThingDefOf.Chemfuel)) {
            processDef.considerBuildingCompResource = true;
        }

        if (def.recipeMaker?.researchPrerequisites != null) {
            processDef.researchPrerequisites = new List<ResearchProjectDef>();
            processDef.researchPrerequisites.AddRange(def.recipeMaker.researchPrerequisites);
        }
        else if (def.recipeMaker?.researchPrerequisite != null) {
            processDef.researchPrerequisites = new List<ResearchProjectDef>();
            processDef.researchPrerequisites.Add(def.recipeMaker.researchPrerequisite);
        }

        InternalDefs.VFEFactory_AutomatedAssembler.GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Add(processDef);
        return processDef;
    }

    public static IEnumerable<PipeSystem.ProcessDef> ImpliedGarageProcesses(bool hotReload = false) {
        List<ThingDef> garageBenchRecipes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            (x.costStuffCount != 0 || x.costList != null)
            //Assembling station only has 4 ports
            && x.costList?.Count < 5
            && x.recipeMaker?.recipeUsers?.Contains(InternalDefs.VVE_GarageBench) == true &&
            !x.defName.Contains("Shell")).ToList();

        _assemblerCount = InternalDefs.VFEFactory_AutomatedAssembler
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;
        foreach (ThingDef def in garageBenchRecipes) {
            yield return ProcessFromFabricationRecipe(InternalDefs.VFEFactory_Assembler_, def, ++_assemblerCount,
                hotReload);
        }
    }

    public static IEnumerable<PipeSystem.ProcessDef> ImpliedFSFBionicProcesses(bool hotReload = false) {
        List<ThingDef> FSFBionicRecipes = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            (x.costStuffCount != 0 || x.costList != null)
            //Assembling station only has 4 ports
            && x.costList?.Count < 5
            && x.recipeMaker?.recipeUsers?.Contains(InternalDefs.FSFBionicsTable) == true &&
            !x.defName.Contains("Shell")).ToList();

        _fabricatorCount = InternalDefs.VFEFactory_AutomatedAssembler
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;
        foreach (ThingDef def in FSFBionicRecipes) {
            yield return ProcessFromFabricationRecipe(InternalDefs.VFEFactory_Assembler_, def, ++_fabricatorCount,
                hotReload);
        }
    }


    public static IEnumerable<PipeSystem.ProcessDef> ImpliedMedicineProcesses(bool hotReload = false) {
        List<ThingDef> medicines = DefDatabase<ThingDef>.AllDefsListForReading.Where(x =>
            (x.costStuffCount != 0 || x.costList != null)
            //medical station only has 3 ports
            && x.costList?.Count < 4
            && x.recipeMaker?.recipeUsers?.Contains(InternalDefs.DrugLab) == true
        ).ToList();
        _granulatorCount = InternalDefs.VFEFactory_MedicineGranulator
            .GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Count;
        foreach (ThingDef def in medicines) {
            yield return ProcessFromMedicineRecipe(InternalDefs.VFEFactory_Medicine_, def, ++_granulatorCount,
                hotReload);
        }
    }

    public static ProcessDef ProcessFromMedicineRecipe(ProcessorTemplateDef tp, ThingDef def, int index,
        bool hotReload = false) {
        string defName = "VFEFactory_MedicineGranulator_" + def.defName;
        ProcessDef processDef = (hotReload
            ? (DefDatabase<ProcessDef>.GetNamed(defName, errorOnFail: false) ?? new ProcessDef())
            : new ProcessDef());
        processDef.defName = defName;
        processDef.label = tp.label.Formatted(def.label);
        processDef.description = tp.description.Formatted(def.label);
        processDef.priorityInBillList = index;
        processDef.spawnOnInteractionCell = tp.spawnOnInteractionCell;
        processDef.autoGrabFromHoppers = tp.autoGrabFromHoppers;
        processDef.autoInputSlots = tp.autoInputSlots;
        processDef.disallowMixing = tp.disallowMixing;

        float calculatedTicks = def.GetStatValueAbstract(StatDefOf.WorkToMake);
        if (calculatedTicks <= 0) {
            calculatedTicks = 3200;
        }

        processDef.ticks = (int)(calculatedTicks * 4);


        if (def.costList != null) {
            List<ProcessDef.Ingredient> ingredientList = new List<ProcessDef.Ingredient>();
            foreach (var cost in def.costList) {
                ingredientList.Add(new ProcessDef.Ingredient {
                    thing = cost.thingDef,
                    countNeeded = cost.count * 4
                });
            }

            processDef.ingredients.AddRange(ingredientList);
        }

        ThingCategoryDef category = null;
        if (def.stuffCategories?.Contains(StuffCategoryDefOf.Fabric) == true) {
            category = ThingCategoryDefOf.Textiles;
        }

        if (category != null) {
            processDef.ingredients.Add(new ProcessDef.Ingredient {
                thingCategory = category,
                countNeeded = def.costStuffCount * 4
            });
        }

        processDef.results = new List<ProcessDef.Result> {
            new ProcessDef.Result {
                thing = def,
                count = 4
            }
        };
        processDef.isFactoryProcess = tp.isFactoryProcess;
        processDef.autoExtract = tp.autoExtract;
        processDef.onlyGrabAndOutputToFactoryHoppers = tp.onlyGrabAndOutputToFactoryHoppers;
        processDef.useFirstIngredientAsOutputStuff = def.costStuffCount != 0;
        processDef.sustainerWhenWorking = tp.sustainerWhenWorking;
        processDef.sustainerDef = tp.sustainerDef;
        processDef.effecterWhenWorking = tp.effecterWhenWorking;
        processDef.effecterDef = tp.effecterDef;
        processDef.maxOutputCount = tp.maxOutputCount;

        if (ModLister.HasActiveModWithName("Vanilla Chemfuel Expanded") &&
            processDef.ingredients.ContainsAny(x => x.thing == ThingDefOf.Chemfuel)) {
            processDef.considerBuildingCompResource = true;
        }

        if (def.recipeMaker?.researchPrerequisites != null) {
            processDef.researchPrerequisites = new List<ResearchProjectDef>();
            processDef.researchPrerequisites.AddRange(def.recipeMaker.researchPrerequisites);
        }
        else if (def.recipeMaker?.researchPrerequisite != null) {
            processDef.researchPrerequisites = new List<ResearchProjectDef>();
            processDef.researchPrerequisites.Add(def.recipeMaker.researchPrerequisite);
        }

        InternalDefs.VFEFactory_MedicineGranulator.GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Add(processDef);
        return processDef;
    }
}