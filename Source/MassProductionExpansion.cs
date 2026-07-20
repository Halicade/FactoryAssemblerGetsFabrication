using System;
using System.Collections.Generic;
using RimWorld;
using HarmonyLib;
using MassProductionExpansion.Defs;
using MassProductionExpansion.Implied;
using VEF.Things;
using Verse;

namespace MassProductionExpansion;

public class MassProductionExpansion : Mod
{
    public MassProductionExpansion(ModContentPack content) : base(content) {
        Harmony harmony = new("Hali.MassProductionExpansion");
        Type patchType = typeof(MassProductionExpansion);
        ChemfuelExpandedActive = ModLister.AnyModActiveNoSuffix(["vanillaexpanded.vchemfuele"]);
        
        harmony.Patch(AccessTools.Method(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve"),
            prefix: new HarmonyMethod(patchType, nameof(FactoryStuffToAssemblerPreFix)));
        LongEventHandler.QueueLongEvent(action: GenerateDrillableMetalsList,
            textKey: null,
            doAsynchronously: true,
            exceptionHandler: null
        );
    }

    public static readonly List<ThingDef> DrillableResources = [];

    private static void GenerateDrillableMetalsList() {
        foreach (ThingDef deepDrillResources in DefDatabase<ThingDef>.AllDefsListForReading) {
            if (deepDrillResources.deepCommonality > 0 && deepDrillResources.deepCountPerPortion > 0) {
                ThingDefExtension thingExtension = deepDrillResources.GetModExtension<ThingDefExtension>();
                if (thingExtension is { allowDeepDrill: false }) {
                    continue;
                }
                DrillableResources.Add(deepDrillResources);
            }
        }
    }

    public static bool ChemfuelExpandedActive;

    //Running this after VE factory cause it seems like a smart idea
    [HarmonyAfter("com.VanillaFurnitureExpandedFactory")]
    public static void FactoryStuffToAssemblerPreFix(bool hotReload = false) {

        foreach (PipeSystem.ProcessDef item in ImpliedAutoloom.ImpliedAutoloomProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        //VE factory 
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_Assembler", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedAssembler,
                     MPEDefOf.MPE_HiTechAutomatedAssembler, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        
        foreach (PipeSystem.ProcessDef item in ImpliedFabrication.AssemblerT2GetsFabricationBench(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedMedicine.ImpliedMedicineProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        /*
        foreach (var item in ImpliedSmelter.ImpliedSmeltingProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
*/
        if (ModLister.AnyModActiveNoSuffix(["oskarpotocki.vanillavehiclesexpanded"])) {
            foreach (PipeSystem.ProcessDef item in ImpliedVehicles.ImpliedGarageProcesses(hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        if (ModLister.AnyModActiveNoSuffix(["FrozenSnowFox.FrozenSnowFoxTweaks"])) {
            foreach (PipeSystem.ProcessDef item in ImpliedBionics.ImpliedFSFBionicProcesses(hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        /* Taking this one out for now
        foreach (var item in ImpliedUpgrader.ImpliedMasonryButcherProcess(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        */

        /*
         
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Drill", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedDrillPlatform,
                     MPEDefOf.MPE_HiTechAutomatedDrillPlatform, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        */

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Smelt", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedSmelter,
                     MPEDefOf.MPE_HiTechAutomatedSmelter, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Oven", 0.75f,
                     FactoryDefOf.VFEFactory_ConveyorOven,
                     MPEDefOf.MPE_HiTechConveyorOven, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        

        
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Neutroamine", 0.75f,
                     FactoryDefOf.VFEFactory_NeutroamineSynthesizer,
                     MPEDefOf.MPE_HiTechNeutroamineSynthesizer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Ammunition", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedAmmunitionPress,
                     MPEDefOf.MPE_HiTechAutomatedAmmunitionPress, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Loom", 0.75f,
                     FactoryDefOf.VFEFactory_Autoloom,
                     MPEDefOf.MPE_HiTechAutoloom, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Forge", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedAlloyForge,
                     MPEDefOf.MPE_HiTechAutomatedAlloyForge, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Distillery", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedDistillery,
                     MPEDefOf.MPE_HiTechAutomatedDistillery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Medicine", 0.75f,
                     FactoryDefOf.VFEFactory_MedicineGranulator,
                     MPEDefOf.MPE_HiTechMedicineGranulator, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }


        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Machining", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedMachiningBay,
                     MPEDefOf.MPE_HiTechAutomatedMachiningBay, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Crematorium", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedCrematorium,
                     MPEDefOf.MPE_HiTechAutomatedCrematorium, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

/*
        InternalDefs.MyNewMachine.GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes
            = InternalDefOf.VFEFactory_AutomatedMachiningBay
                .GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes;
                */
    }
}