using PipeSystem;
using RimWorld;
using Verse;

namespace MassProductionExpansion.ResultWorker;

public class Metals : ProcessDef.ResultWorker
{
    public ThingDef cachedMetal;

    public override ThingDef GetResult(Process process) {


        ThingDef result = MassProductionExpansion.DrillableResources.RandomElementByWeight(x => x.deepCommonality);
        if (result != null) {
            cachedMetal = result;
            return result;
        }

        cachedMetal = ThingDefOf.Steel;
        return this.result.thing;
    }

    public override int GetCount(Process process) {
        return cachedMetal?.deepCountPerPortion ?? result.count;
    }
}