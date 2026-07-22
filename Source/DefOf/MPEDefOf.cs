using RimWorld;
using Verse;

namespace MassProductionExpansion.Defs;

[DefOf]
public class MPEDefOf
{
    public static ThingDef FabricationBench;

    public static ThingDef DrugLab;
    
    // T2
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
    
    // T3
    public static ThingDef MPE_SmartAutomatedDrillPlatform;
    public static ThingDef MPE_SmartAutomatedSmelter;
    public static ThingDef MPE_SmartAutomatedBiofuelRefinery;
    public static ThingDef MPE_SmartAutomatedMasonrySaw;
    public static ThingDef MPE_SmartAutomatedMincer;
    public static ThingDef MPE_SmartConveyorOven;
    public static ThingDef MPE_SmartAutomatedAssembler;
    public static ThingDef MPE_SmartNeutroamineSynthesizer;
    public static ThingDef MPE_SmartAutomatedAmmunitionPress;
    public static ThingDef MPE_SmartAutoloom;
    public static ThingDef MPE_SmartAutomatedAlloyForge;
    public static ThingDef MPE_SmartAutomatedDistillery;
    public static ThingDef MPE_SmartMedicineGranulator;
    public static ThingDef MPE_SmartAutomatedMachiningBay;
    public static ThingDef MPE_SmartAutomatedCrematorium;
    
    
    public static ProcessorTemplateDef MPE_Assembler_;
    public static ProcessorTemplateDef MPE_Medicine_;


    //Required so the game doesn't yell at you if not loaded
    [MayRequire("oskarpotocki.vanillavehiclesexpanded")]
    public static ThingDef VVE_GarageBench;

    [MayRequire("FrozenSnowFox.FrozenSnowFoxTweaks")]
    public static ThingDef FSFBionicsTable;

    static MPEDefOf() {
        DefOfHelper.EnsureInitializedInCtor(typeof(MPEDefOf));
    }

}