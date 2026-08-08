using System;
using System.Collections.Generic;
using RimWorld;
using HarmonyLib;
using MassProductionExpansion.Defs;
using MassProductionExpansion.Implied;
using PipeSystem;
using VEF.Things;
using Verse;

namespace MassProductionExpansion;

public class MassProductionExpansion : Mod
{
    public MassProductionExpansion(ModContentPack content) : base(content) {
        Harmony harmony = new("Hali.MassProductionExpansion");
        Type patchType = typeof(MassProductionExpansion);
        ChemfuelExpandedActive = ModLister.AnyModActiveNoSuffix(["vanillaexpanded.vchemfuele"]);
        VECookingActive = ModLister.AnyModActiveNoSuffix(["VanillaExpanded.VCookE"]);
        CEActive = ModLister.AnyModActiveNoSuffix(["ceteam.combatextended"]);

        harmony.Patch(AccessTools.Method(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve"),
            prefix: new HarmonyMethod(patchType, nameof(FactoryStuffToAssemblerPreFix)));

        harmony.Patch(AccessTools.Method(typeof(Process), nameof(Process.HandleIngredientsAndQuality)),
            postfix: new HarmonyMethod(patchType, nameof(IncreaseQualityPostFix)));

        if (CEActive) {
            harmony.Patch(AccessTools.Method("CombatExtended.AmmoInjector:Inject"),
                postfix: new HarmonyMethod(patchType, nameof(CeAmmunitionCreator)));
        }

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

    public static void IncreaseQualityPostFix(Thing outThing, Process __instance) {
        var itemQuality = outThing.TryGetComp<CompQuality>();
        if (itemQuality == null) {
            return;
        }

        CompAffectedByFacilities linkables =
            __instance.advancedProcessor.parent.TryGetComp<CompAffectedByFacilities>();

        if (linkables == null) {
            return;
        }

        bool itemCanBeUpgraded = false;
        foreach (var linkedThings in linkables.LinkedFacilitiesListForReading) {
            if (linkedThings.def == MPEDefOf.MPE_MechUplink) {
                itemCanBeUpgraded = true;
                break;
            }
        }

        if (!itemCanBeUpgraded) {
            return;
        }

        if (!outThing.TryGetQuality(out QualityCategory qualityCategory)) {
            return;
        }

        int currentQuality = (int)qualityCategory;
        if (Rand.Bool) {
            currentQuality++;
            if (Rand.Bool) {
                currentQuality++;
            }
        }

        currentQuality = Math.Min(currentQuality, 6);
        itemQuality.SetQuality((QualityCategory)currentQuality, null);
    }


    public static bool ChemfuelExpandedActive;
    public static bool VECookingActive;
    public static bool CEActive;

    //Needs to run after VE factory
    [HarmonyPriority(Priority.Low)]
    public static void FactoryStuffToAssemblerPreFix(bool hotReload = false) {
        // T2 Upgrades

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Smelt", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedSmelter,
                     MPEDefOf.MPE_HiTechAutomatedSmelter, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Assembler_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedAssembler,
                     MPEDefOf.MPE_HiTechAutomatedAssembler, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Refinery_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedBiofuelRefinery,
                     MPEDefOf.MPE_HiTechAutomatedBiofuelRefinery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Masonry_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedMasonrySaw,
                     MPEDefOf.MPE_HiTechAutomatedMasonrySaw, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Mincer_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedMincer,
                     MPEDefOf.MPE_HiTechAutomatedMincer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Oven_", 0.5f,
                     FactoryDefOf.VFEFactory_ConveyorOven,
                     MPEDefOf.MPE_HiTechConveyorOven, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Neutroamine_", 0.5f,
                     FactoryDefOf.VFEFactory_NeutroamineSynthesizer,
                     MPEDefOf.MPE_HiTechNeutroamineSynthesizer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        if (!CEActive) {
            foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Ammunition_", 0.5f,
                         FactoryDefOf.VFEFactory_AutomatedAmmunitionPress,
                         MPEDefOf.MPE_HiTechAutomatedAmmunitionPress, hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Loom_", 0.5f,
                     FactoryDefOf.VFEFactory_Autoloom,
                     MPEDefOf.MPE_HiTechAutoloom, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Forge_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedAlloyForge,
                     MPEDefOf.MPE_HiTechAutomatedAlloyForge, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Distillery_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedDistillery,
                     MPEDefOf.MPE_HiTechAutomatedDistillery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        if (VECookingActive) {
            foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Cannery_", 0.5f,
                         FactoryDefOf.VFEFactory_AutomatedCannery,
                         MPEDefOf.MPE_HiTechAutomatedCannery, hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Medicine", 0.5f,
                     FactoryDefOf.VFEFactory_MedicineGranulator,
                     MPEDefOf.MPE_HiTechMedicineGranulator, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Machining_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedMachiningBay,
                     MPEDefOf.MPE_HiTechAutomatedMachiningBay, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Crematorium_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedCrematorium,
                     MPEDefOf.MPE_HiTechAutomatedCrematorium, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        // Adding custom recipes

        foreach (PipeSystem.ProcessDef item in ImpliedAutoloom.ImpliedAutoloomProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (PipeSystem.ProcessDef item in ImpliedFabrication.AssemblerT2GetsFabricationBench(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        /*
         Not needed
        foreach (var item in ImpliedMedicine.ImpliedMedicineProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        */

        foreach (var item in ImpliedStone.ImpliedStoneProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedSmelter.ImpliedSmelterProcess(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        if (ModLister.AnyModActiveNoSuffix(["oskarpotocki.vanillavehiclesexpanded"])) {
            foreach (PipeSystem.ProcessDef item in ImpliedVehicles.ImpliedGarageProcesses(hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        /*
        if (ModLister.AnyModActiveNoSuffix(["FrozenSnowFox.FrozenSnowFoxTweaks"])) {
            foreach (PipeSystem.ProcessDef item in ImpliedBionics.ImpliedFSFBionicProcesses(hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }
        */


        // T3 upgrades
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedDrillPlatform,
                     MPEDefOf.MPE_SmartAutomatedDrillPlatform, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedSmelter,
                     MPEDefOf.MPE_SmartAutomatedSmelter, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedBiofuelRefinery,
                     MPEDefOf.MPE_SmartAutomatedBiofuelRefinery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedMasonrySaw,
                     MPEDefOf.MPE_SmartAutomatedMasonrySaw, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedMincer,
                     MPEDefOf.MPE_SmartAutomatedMincer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }


        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechConveyorOven,
                     MPEDefOf.MPE_SmartConveyorOven, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedAssembler,
                     MPEDefOf.MPE_SmartAutomatedAssembler, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechNeutroamineSynthesizer,
                     MPEDefOf.MPE_SmartNeutroamineSynthesizer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        if (!CEActive) {
            foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                         MPEDefOf.MPE_HiTechAutomatedAmmunitionPress,
                         MPEDefOf.MPE_SmartAutomatedAmmunitionPress, hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutoloom,
                     MPEDefOf.MPE_SmartAutoloom, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedAlloyForge,
                     MPEDefOf.MPE_SmartAutomatedAlloyForge, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedDistillery,
                     MPEDefOf.MPE_SmartAutomatedDistillery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechMedicineGranulator,
                     MPEDefOf.MPE_SmartMedicineGranulator, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedMachiningBay,
                     MPEDefOf.MPE_SmartAutomatedMachiningBay, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedCrematorium,
                     MPEDefOf.MPE_SmartAutomatedCrematorium, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        if (VECookingActive) {
            foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                         MPEDefOf.MPE_HiTechAutomatedCannery,
                         MPEDefOf.MPE_SmartAutomatedCannery, hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        if (ModsConfig.OdysseyActive) {
            foreach (var item in ImpliedUpgrader.ImpliedTransferProcess("MPET3G_", 1f,
                         MPEDefOf.MPE_SmartAutomatedMachiningBay,
                         MPEDefOf.MPE_GravtechAssembler, MPEDefOf.MPE_Grav_, hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
            
            foreach (var item in ImpliedUpgrader.ImpliedTransferProcess("MPET3G_", 1f,
                         MPEDefOf.MPE_SmartAutomatedAssembler,
                         MPEDefOf.MPE_GravtechAssembler, MPEDefOf.MPE_Grav_, hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
            
            foreach (var item in ImpliedUpgrader.ImpliedTransferProcess("MPET3G_", 1f,
                         MPEDefOf.MPE_SmartAutoloom,
                         MPEDefOf.MPE_GravtechAssembler, MPEDefOf.MPE_Grav_, hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
            
        }
    }

    // Need to run after Munitions Industries for VFE - Factory
    [HarmonyPriority(Priority.Low)]
    public static void CeAmmunitionCreator() {
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Ammunition_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedAmmunitionPress,
                     MPEDefOf.MPE_HiTechAutomatedAmmunitionPress)) {
            DefGenerator.AddImpliedDef(item);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedAmmunitionPress,
                     MPEDefOf.MPE_SmartAutomatedAmmunitionPress)) {
            DefGenerator.AddImpliedDef(item);
        }
    }
}