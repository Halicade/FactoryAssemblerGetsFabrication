using PipeSystem;
using RimWorld;
using VanillaFurnitureExpandedFactory;
using Verse;

namespace MassProductionExpansion.Building;

public class T2FactoryHopper : Building_FactoryHopper
{
    private Graphic currentGraphic;

    public override Graphic Graphic => currentGraphic ?? base.Graphic;

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
        base.SpawnSetup(map, respawningAfterLoad);
        currentGraphic = CheckForT2Factories();
    }


    public override void TickLong() {
        //base.TickLong();
        Graphic graphic = CheckForT2Factories();
        if (graphic != currentGraphic) {
            currentGraphic = graphic;
            base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlagDefOf.Things);
        }
    }

    public Graphic CheckForT2Factories() {
        if (base.Map != null) {
            foreach (IntVec3 edgeCell in this.OccupiedRect().ExpandedBy(1).EdgeCells) {
                if (!edgeCell.InBounds(base.Map)) {
                    continue;
                }

                foreach (Thing thing in edgeCell.GetThingList(base.Map)) {
                    if (!thing.def.IsEdifice()) {
                        continue;
                    }

                    CompAdvancedResourceProcessor compAdvancedResourceProcessor =
                        thing.TryGetComp<CompAdvancedResourceProcessor>();
                    if (compAdvancedResourceProcessor == null) {
                        continue;
                    }

                    if (thing.def.hasInteractionCell &&
                        thing.Position + thing.def.interactionCellOffset.RotatedBy(thing.Rotation) == base.Position) {
                        return GraphicsCache.T2HopperOutput;
                    }

                    if (compAdvancedResourceProcessor.Process == null ||
                        compAdvancedResourceProcessor.Process.Def.autoInputSlots.NullOrEmpty()) {
                        continue;
                    }

                    foreach (IntVec3 autoInputSlot in compAdvancedResourceProcessor.Process.Def.autoInputSlots) {
                        if (thing.Position + autoInputSlot.RotatedBy(thing.Rotation) == base.Position) {
                            return GraphicsCache.T2HopperInput;
                        }
                    }
                }
            }
        }

        return GraphicsCache.T2HopperNormal;
    }
}