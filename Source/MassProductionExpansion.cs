using System;
using RimWorld;
using HarmonyLib;
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
        ChemfuelExpandedActive=ModLister.AnyModActiveNoSuffix(["vanillaexpanded.vchemfuele"]);

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
/*
        InternalDefs.MyNewMachine.GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes
            = InternalDefOf.VFEFactory_AutomatedMachiningBay
                .GetCompProperties<CompProperties_AdvancedResourceProcessor>().processes;
                */
    }
}