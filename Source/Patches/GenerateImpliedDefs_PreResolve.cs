using HarmonyLib;
using MassProductionExpansion.Defs;
using MassProductionExpansion.Implied;
using RimWorld;
using Verse;

namespace MassProductionExpansion.Patches;

[HarmonyPatch(typeof(DefGenerator), nameof(DefGenerator.GenerateImpliedDefs_PreResolve))]
public static class GenerateImpliedDefs_PreResolve
{
    [HarmonyPriority(Priority.Low)]
    [HarmonyPrefix]
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

        if (!global::MassProductionExpansion.MassProductionExpansion.CEActive) {
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

        if (global::MassProductionExpansion.MassProductionExpansion.VECookingActive) {
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

        if (!MassProductionExpansion.UltratechActive) {
            return;
        }

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

        if (!global::MassProductionExpansion.MassProductionExpansion.CEActive) {
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

        if (MassProductionExpansion.VECookingActive) {
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
}