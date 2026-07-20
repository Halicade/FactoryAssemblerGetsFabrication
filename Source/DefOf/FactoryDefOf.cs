using RimWorld;
using VanillaFurnitureExpandedFactory;
using Verse;

namespace MassProductionExpansion.Defs;

[DefOf]
public class FactoryDefOf
{
    public static ThingDef VFEFactory_AutomatedAssembler;
    public static ThingDef VFEFactory_MedicineGranulator;
    public static AutoloomProcessTemplateDef VFEFactory_Autoloom_;
    public static ThingDef VFEFactory_AutomatedMasonrySaw;
    public static ThingDef VFEFactory_AutomatedMincer;

    public static ThingDef VFEFactory_AutomatedDrillPlatform;
    public static ThingDef VFEFactory_AutomatedSmelter;
    public static ThingDef VFEFactory_AutomatedBiofuelRefinery;
    public static ThingDef VFEFactory_ConveyorOven;
    public static ThingDef VFEFactory_NeutroamineSynthesizer;
    public static ThingDef VFEFactory_AutomatedAmmunitionPress;
    public static ThingDef VFEFactory_Autoloom;
    public static ThingDef VFEFactory_AutomatedAlloyForge;
    public static ThingDef VFEFactory_AutomatedDistillery;
    public static ThingDef VFEFactory_AutomatedMachiningBay;
    public static ThingDef VFEFactory_AutomatedCrematorium;
    

    static FactoryDefOf() {
        DefOfHelper.EnsureInitializedInCtor(typeof(FactoryDefOf));
    }
}