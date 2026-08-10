using System.Collections.Generic;
using PipeSystem;
using RimWorld;
using Verse;

namespace MassProductionExpansion.Pipenet;

//Copy of PipeSystem.CompProperties_SpawnerOrNet but spawner will not spawn if net is full

public class CompExtractToNet : CompResource
{
    private CompCanBeDormant compCanBeDormant;

    private CompPowerTrader compPower;

    private CompResource compResource;

    private CompRefuelable compFuel;

    private int ticksUntilSpawn;

    private new CompProperties_SpawnToNet Props => (CompProperties_SpawnToNet)props;

    public override void PostSpawnSetup(bool respawningAfterLoad) {
        base.PostSpawnSetup(respawningAfterLoad);
        compPower = parent.GetComp<CompPowerTrader>();
        compResource = parent.GetComp<CompResource>();
        compCanBeDormant = parent.GetComp<CompCanBeDormant>();
        compFuel = parent.GetComp<CompRefuelable>();
        if (!respawningAfterLoad) {
            ticksUntilSpawn = Props.spawnIntervalRange.RandomInRange;
        }
    }

    public override string CompInspectStringExtra() {
        if (!parent.Spawned) {
            return null;
        }

        if (compPower is not { PowerOn: true } || compFuel is not { HasFuel: true }) {
            return base.CompInspectStringExtra();
        }

        return base.CompInspectStringExtra() + "\n" +
               "NextSpawnedItemIn".Translate(GenLabel.ThingLabel(Props.thingToSpawn, null, Props.spawnCount))
                   .Resolve() + ": " + ticksUntilSpawn.ToStringTicksToPeriod().Colorize(ColoredText.DateTimeColor);
    }

    public override void PostExposeData() {
        Scribe_Values.Look(ref ticksUntilSpawn,
            (Props.saveKeysPrefix.NullOrEmpty() ? null : (Props.saveKeysPrefix + "_")) + "ticksUntilSpawn", 0);
    }

    public override IEnumerable<Gizmo> CompGetGizmosExtra() {
        foreach (Gizmo item in base.CompGetGizmosExtra()) {
            yield return item;
        }

        if (DebugSettings.ShowDevGizmos) {
            yield return new Command_Action {
                action = delegate { ticksUntilSpawn = 50; },
                defaultLabel = "Spawn now",
                defaultDesc = "Spawn now"
            };
        }
    }

    public override void CompTickInterval(int delta) {
        TickInterval(delta);
    }

    public override void CompTickRare() {
        TickInterval(250);
    }

    public override void CompTickLong() {
        TickInterval(2000);
    }

    private void TickInterval(int interval) {
        if (parent.Spawned && (compCanBeDormant == null || compCanBeDormant.Awake) &&
            !parent.Position.Fogged(parent.Map) && (compPower == null || compPower.PowerOn) &&
            (compFuel == null || compFuel.HasFuel)) {
            ticksUntilSpawn -= interval;
            if (ticksUntilSpawn <= 0) {
                ticksUntilSpawn = Props.spawnIntervalRange.RandomInRange;
                TryDoSpawn();
            }
        }
    }

    private void TryDoSpawn() {
        if (!parent.Spawned) {
            return;
        }


        PipeNet pipeNet = compResource.PipeNet;
        if (pipeNet.AvailableCapacity <= 0) {
            return;
        }
        
        if (pipeNet.AvailableCapacity >= (float)Props.spawnCount) {
            pipeNet.DistributeAmongStorage(Props.spawnCount, out _);

            return;
        }

        pipeNet.DistributeAmongStorage(pipeNet.AvailableCapacity, out _);
    }
}