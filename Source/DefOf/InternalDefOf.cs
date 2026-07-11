using RimWorld;
using VanillaFurnitureExpandedFactory;
using Verse;

namespace MassProductionExpansion.Defs;

[DefOf]
public class InternalDefOf
{
    public static ThingDef FabricationBench;

    public static ThingDef DrugLab;


    public static ThingDef MPE_T2AutomatedMasonrySaw;
    public static ThingDef MPE_HiTechAutomatedDrillPlatform;
    public static ThingDef MPE_HiTechAutomatedSmelter;
    public static ThingDef MPE_HiTechAutomatedBiofuelRefinery;
    public static ThingDef MPE_HiTechAutomatedMasonrySaw;
    public static ThingDef MPE_HiTechAutomatedMincer;
    public static ThingDef MPE_HiTechConveyorOven;
    public static ThingDef MPE_HiTechAutomatedAssembler;
    public static ThingDef MPE_HiTechNeutroamineSynthesizer;
    public static ThingDef MPE_HiTechAutomatedAmmunitionPress;
    public static ThingDef MPE_HiTechAutoloom;
    public static ThingDef MPE_HiTechAutomatedAlloyForge;
    public static ThingDef MPE_HiTechAutomatedDistillery;
    public static ThingDef MPE_HiTechMedicineGranulator;
    public static ThingDef MPE_HiTechAutomatedMachiningBay;
    public static ThingDef MPE_HiTechAutomatedCrematorium;


    //Required so the game doesn't yell at you if not loaded
    [MayRequire("oskarpotocki.vanillavehiclesexpanded")]
    public static ThingDef VVE_GarageBench;

    [MayRequire("FrozenSnowFox.FrozenSnowFoxTweaks")]
    public static ThingDef FSFBionicsTable;

    static InternalDefOf() {
        DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
    }
}