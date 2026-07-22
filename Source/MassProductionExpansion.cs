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

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Assembler", 0.5f,
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

        foreach (var item in ImpliedStone.ImpliedStoneProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

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

        // T2 Upgrades
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Smelt", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedSmelter,
                     MPEDefOf.MPE_HiTechAutomatedSmelter, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Refinery", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedBiofuelRefinery,
                     MPEDefOf.MPE_HiTechAutomatedBiofuelRefinery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Masonry", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedMasonrySaw,
                     MPEDefOf.MPE_HiTechAutomatedMasonrySaw, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Mincer", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedMincer,
                     MPEDefOf.MPE_HiTechAutomatedMincer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Oven", 0.5f,
                     FactoryDefOf.VFEFactory_ConveyorOven,
                     MPEDefOf.MPE_HiTechConveyorOven, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Neutroamine", 0.5f,
                     FactoryDefOf.VFEFactory_NeutroamineSynthesizer,
                     MPEDefOf.MPE_HiTechNeutroamineSynthesizer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Ammunition", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedAmmunitionPress,
                     MPEDefOf.MPE_HiTechAutomatedAmmunitionPress, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Loom", 0.5f,
                     FactoryDefOf.VFEFactory_Autoloom,
                     MPEDefOf.MPE_HiTechAutoloom, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Forge", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedAlloyForge,
                     MPEDefOf.MPE_HiTechAutomatedAlloyForge, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Distillery", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedDistillery,
                     MPEDefOf.MPE_HiTechAutomatedDistillery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        /*
         Not needed
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Medicine", 0.5f,
                     FactoryDefOf.VFEFactory_MedicineGranulator,
                     MPEDefOf.MPE_HiTechMedicineGranulator, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        */


        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Machining", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedMachiningBay,
                     MPEDefOf.MPE_HiTechAutomatedMachiningBay, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Crematorium", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedCrematorium,
                     MPEDefOf.MPE_HiTechAutomatedCrematorium, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }


        // T3 upgrades
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Drill", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedDrillPlatform,
                     MPEDefOf.MPE_SmartAutomatedDrillPlatform, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Smelt", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedSmelter,
                     MPEDefOf.MPE_SmartAutomatedSmelter, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Refinery", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedBiofuelRefinery,
                     MPEDefOf.MPE_SmartAutomatedBiofuelRefinery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Masonry", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedMasonrySaw,
                     MPEDefOf.MPE_SmartAutomatedMasonrySaw, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Mincer", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedMincer,
                     MPEDefOf.MPE_SmartAutomatedMincer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }


        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Oven", 0.5f,
                     MPEDefOf.MPE_HiTechConveyorOven,
                     MPEDefOf.MPE_SmartConveyorOven, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Assmebler", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedAssembler,
                     MPEDefOf.MPE_SmartAutomatedAssembler, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Neutroamine", 0.5f,
                     MPEDefOf.MPE_HiTechNeutroamineSynthesizer,
                     MPEDefOf.MPE_SmartNeutroamineSynthesizer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Ammunition", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedAmmunitionPress,
                     MPEDefOf.MPE_SmartAutomatedAmmunitionPress, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Loom", 0.5f,
                     MPEDefOf.MPE_HiTechAutoloom,
                     MPEDefOf.MPE_SmartAutoloom, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Forge", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedAlloyForge,
                     MPEDefOf.MPE_SmartAutomatedAlloyForge, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Distillery", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedDistillery,
                     MPEDefOf.MPE_SmartAutomatedDistillery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Medicine", 0.5f,
                     MPEDefOf.MPE_HiTechMedicineGranulator,
                     MPEDefOf.MPE_SmartMedicineGranulator, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Machining", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedMachiningBay,
                     MPEDefOf.MPE_SmartAutomatedMachiningBay, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T3Crematorium", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedCrematorium,
                     MPEDefOf.MPE_SmartAutomatedCrematorium, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
    }
}