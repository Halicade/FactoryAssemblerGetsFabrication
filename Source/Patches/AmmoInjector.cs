using System.Reflection;
using HarmonyLib;
using MassProductionExpansion.Defs;
using MassProductionExpansion.Implied;
using RimWorld;

namespace MassProductionExpansion.Patches;

[HarmonyPatch]
public static class AmmoInjector
{

    static bool Prepare() => MassProductionExpansion.CEActive;

    static MethodBase TargetMethod() =>
        AccessTools.Method("CombatExtended.AmmoInjector:Inject");

    [HarmonyPostfix]
    [HarmonyPriority(Priority.Low)]
    public static void CeAmmunitionCreator() {
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPE_T2Ammunition_", 0.5f,
                     FactoryDefOf.VFEFactory_AutomatedAmmunitionPress,
                     MPEDefOf.MPE_HiTechAutomatedAmmunitionPress)) {
            DefGenerator.AddImpliedDef(item);
        }

        if (!MassProductionExpansion.UltratechActive) {
            return;
        }
        
        foreach (var item in ImpliedUpgrader.ImpliedGenericProcess("MPET3_", 0.5f,
                     MPEDefOf.MPE_HiTechAutomatedAmmunitionPress,
                     MPEDefOf.MPE_SmartAutomatedAmmunitionPress)) {
            DefGenerator.AddImpliedDef(item);
        }
    }
}