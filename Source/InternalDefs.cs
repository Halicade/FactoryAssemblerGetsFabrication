using RimWorld;
using Verse;

namespace MassProductionExpansion;

[DefOf]
public class InternalDefs
{
    public static ThingDef FabricationBench;

    public static ThingDef DrugLab;

    public static ThingDef VFEFactory_AutomatedAssembler;

    public static ThingDef VFEFactory_MedicineGranulator;

    public static ProcessorTemplateDef VFEFactory_Assembler_;

    public static ProcessorTemplateDef VFEFactory_Medicine_;

    //Required so the game doesn't yell at you if not loaded
    [MayRequire("oskarpotocki.vanillavehiclesexpanded")]
    public static ThingDef VVE_GarageBench;

    [MayRequire("FrozenSnowFox.FrozenSnowFoxTweaks")]
    public static ThingDef FSFBionicsTable;

    static InternalDefs() {
        DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefs));
    }
}