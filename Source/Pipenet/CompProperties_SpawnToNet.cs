using PipeSystem;
using Verse;

namespace MassProductionExpansion.Pipenet;

public class CompProperties_SpawnToNet : CompProperties_Resource
{
    public string saveKeysPrefix;

    public int spawnCount = 1;

    public IntRange spawnIntervalRange = new(100, 100);

    public ThingDef thingToSpawn;


    public CompProperties_SpawnToNet() {
        compClass = typeof(CompExtractToNet);
    }
}