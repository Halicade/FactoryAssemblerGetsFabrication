using System;
using RimWorld;
using HarmonyLib;
using MassProductionExpansion.Defs;
using MassProductionExpansion.Implied;
using Verse;

namespace MassProductionExpansion;

public class MassProductionExpansion : Mod
{
    public MassProductionExpansion(ModContentPack content) : base(content) {
        Harmony harmony = new("Hali.MassProductionExpansion");
        Type patchType = typeof(MassProductionExpansion);

        harmony.Patch(AccessTools.Method(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve"),
            prefix: new HarmonyMethod(patchType, nameof(FactoryStuffToAssemblerPostFix)));
    }


    public static int AutoLoomCount = 0;
    public static int GranulatorCount = 0;
    public static int FabricatorCount = 0;
    public static int AssemblerCount = 0;

    private static bool _vehiclesActive;
    private static bool _fsfTweaksActive;
    public static bool ChemfuelExpandedActive;

    //Running this after VE factory cause it seems like a smart idea
    [HarmonyAfter("com.VanillaFurnitureExpandedFactory")]
    public static void FactoryStuffToAssemblerPostFix(bool hotReload = false) {
        _vehiclesActive = ModLister.AnyModActiveNoSuffix(["oskarpotocki.vanillavehiclesexpanded"]);
        _fsfTweaksActive = ModLister.AnyModActiveNoSuffix(["FrozenSnowFox.FrozenSnowFoxTweaks"]);
        ChemfuelExpandedActive = ModLister.AnyModActiveNoSuffix(["vanillaexpanded.vchemfuele"]);

        foreach (PipeSystem.ProcessDef item in ImpliedAutoloom.ImpliedAutoloomProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }


        foreach (PipeSystem.ProcessDef item in ImpliedFabrication.ImpliedFabricationProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedMedicine.ImpliedMedicineProcesses(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        if (_vehiclesActive) {
            foreach (PipeSystem.ProcessDef item in ImpliedVehicles.ImpliedGarageProcesses(hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        if (_fsfTweaksActive) {
            foreach (PipeSystem.ProcessDef item in ImpliedBionics.ImpliedFSFBionicProcesses(hotReload)) {
                DefGenerator.AddImpliedDef(item, hotReload);
            }
        }

        foreach (var item in ImpliedUpgrader.ImpliedMasonryButcherProcess(hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Drill", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedDrillPlatform,
                     InternalDefOf.MPE_HiTechAutomatedDrillPlatform, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Smelt", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedSmelter,
                     InternalDefOf.MPE_HiTechAutomatedSmelter, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Biofuel", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedBiofuelRefinery,
                     InternalDefOf.MPE_HiTechAutomatedBiofuelRefinery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Oven", 0.75f,
                     FactoryDefOf.VFEFactory_ConveyorOven,
                     InternalDefOf.MPE_HiTechConveyorOven, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_Assembler", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedAssembler,
                     InternalDefOf.MPE_HiTechAutomatedAssembler, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }
        
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Neutroamine", 0.75f,
                     FactoryDefOf.VFEFactory_NeutroamineSynthesizer,
                     InternalDefOf.MPE_HiTechNeutroamineSynthesizer, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Ammunition", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedAmmunitionPress,
                     InternalDefOf.MPE_HiTechAutomatedAmmunitionPress, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Loom", 0.75f,
                     FactoryDefOf.VFEFactory_Autoloom,
                     InternalDefOf.MPE_HiTechAutoloom, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Forge", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedAlloyForge,
                     InternalDefOf.MPE_HiTechAutomatedAlloyForge, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Distillery", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedDistillery,
                     InternalDefOf.MPE_HiTechAutomatedDistillery, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Medicine", 0.75f,
                     FactoryDefOf.VFEFactory_MedicineGranulator,
                     InternalDefOf.MPE_HiTechMedicineGranulator, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }


        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Machining", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedMachiningBay,
                     InternalDefOf.MPE_HiTechAutomatedMachiningBay, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Crematorium", 0.75f,
                     FactoryDefOf.VFEFactory_AutomatedCrematorium,
                     InternalDefOf.MPE_HiTechAutomatedCrematorium, hotReload)) {
            DefGenerator.AddImpliedDef(item, hotReload);
        }

/*
        InternalDefs.MyNewMachine.GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes
            = InternalDefOf.VFEFactory_AutomatedMachiningBay
                .GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes;
                */
    }
}