using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using VEF.Things;
using Verse;

namespace MassProductionExpansion;

public class MassProductionExpansion : Mod
{
    public MassProductionExpansion(ModContentPack content) : base(content) {
        Harmony harmony = new Harmony("Hali.MassProductionExpansion");
        UltratechActive = ModLister.AnyModActiveNoSuffix(["OskarPotocki.VFE.Ultratech"]);
        ChemfuelExpandedActive = ModLister.AnyModActiveNoSuffix(["vanillaexpanded.vchemfuele"]);
        VECookingActive = ModLister.AnyModActiveNoSuffix(["VanillaExpanded.VCookE"]);
        CEActive = ModLister.AnyModActiveNoSuffix(["ceteam.combatextended"]);


        harmony.PatchAll(Assembly.GetExecutingAssembly());
        /*
        harmony.Patch(AccessTools.Method(typeof(DefGenerator), "GenerateImpliedDefs_PreResolve"),
            prefix: new HarmonyMethod(patchType, nameof(Patch_FactoryAssemblerPrefix.FactoryStuffToAssemblerPreFix)));

        harmony.Patch(AccessTools.Method(typeof(Process), nameof(Process.HandleIngredientsAndQuality)),
            postfix: new HarmonyMethod(patchType, nameof(IncreaseQuality.IncreaseQualityPostFix)));

        if (CEActive) {
            harmony.Patch(AccessTools.Method("CombatExtended.AmmoInjector:Inject"),
                postfix: new HarmonyMethod(patchType, nameof(CeAmmunition.CeAmmunitionCreator)));
        }
        */

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

    public static bool UltratechActive;
    public static bool ChemfuelExpandedActive;
    public static bool VECookingActive;
    public static bool CEActive;

    //Needs to run after VE factory

    // Need to run after Munitions Industries for VFE - Factory




}