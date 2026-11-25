using RimWorld;
using Verse;

namespace MassProductionExpansion.Defs;

[DefOf]
public class MPEDefOf
{
    public static ThingDef FabricationBench;
    public static ThingDef ElectricSmithy;
    public static ThingDef FueledSmithy;
    public static ThingDef DrugLab;
    public static ThingDef MPE_MechUplink;

    [MayRequire("ceteam.combatextended")]
    public static StuffCategoryDef Steeled;
    
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
    
    [MayRequire("VanillaExpanded.VCookE")]
    public static ThingDef MPE_HiTechAutomatedCannery;
    
    // T3
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedDrillPlatform;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedSmelter;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedBiofuelRefinery;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedMasonrySaw;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedMincer;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartConveyorOven;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedAssembler;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartNeutroamineSynthesizer;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedAmmunitionPress;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutoloom;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedAlloyForge;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedDistillery;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartMedicineGranulator;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedMachiningBay;
    [MayRequire("OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedCrematorium;
    [MayRequire("VanillaExpanded.VCookE,OskarPotocki.VFE.Ultratech")]
    public static ThingDef MPE_SmartAutomatedCannery;

    [MayRequireOdyssey] 
    public static ThingDef MPE_GravtechAssembler;
    
    
    public static ProcessorTemplateDef MPE_Assembler_;
    public static ProcessorTemplateDef MPE_Medicine_;
    public static ProcessorTemplateDef MPE_Smelter_;
    public static ProcessorTemplateDef MPE_Machining_;
    [MayRequireOdyssey]
    public static ProcessorTemplateDef MPE_Grav_;


    //Required so the game doesn't yell at you if not loaded
    [MayRequire("oskarpotocki.vanillavehiclesexpanded")]
    public static ThingDef VVE_GarageBench;

    [MayRequire("FrozenSnowFox.FrozenSnowFoxTweaks")]
    public static ThingDef FSFBionicsTable;

    static MPEDefOf() {
        DefOfHelper.EnsureInitializedInCtor(typeof(MPEDefOf));
    }

}