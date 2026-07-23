using PipeSystem;
using RimWorld;
using VanillaFurnitureExpandedFactory;
using Verse;

namespace MassProductionExpansion.Building;

public class T3FactoryHopper: Building_FactoryHopper
{
    private Graphic currentGraphic;

    public override Graphic Graphic => currentGraphic ?? base.Graphic;

    public override void SpawnSetup(Map map, bool respawningAfterLoad) {
        base.SpawnSetup(map, respawningAfterLoad);
        currentGraphic = T3();
    }


    public override void TickLong() {
        //base.TickLong();
        Graphic graphic = T3();
        if (graphic != currentGraphic) {
            currentGraphic = graphic;
            Map.mapDrawer.MapMeshDirty(Position, MapMeshFlagDefOf.Things);
        }
    }

    private Graphic T3() {
        if (Map != null) {
            foreach (IntVec3 edgeCell in this.OccupiedRect().ExpandedBy(1).EdgeCells) {
                if (!edgeCell.InBounds(Map)) {
                    continue;
                }

                foreach (Thing thing in edgeCell.GetThingList(Map)) {
                    if (!thing.def.IsEdifice()) {
                        continue;
                    }

                    CompAdvancedResourceProcessor compAdvancedResourceProcessor =
                        thing.TryGetComp<CompAdvancedResourceProcessor>();
                    if (compAdvancedResourceProcessor == null) {
                        continue;
                    }

                    if (thing.def.hasInteractionCell &&
                        thing.Position + thing.def.interactionCellOffset.RotatedBy(thing.Rotation) == Position) {
                        return GraphicsCache.T3HopperOutput;
                    }

                    if (compAdvancedResourceProcessor.Process == null ||
                        compAdvancedResourceProcessor.Process.Def.autoInputSlots.NullOrEmpty()) {
                        continue;
                    }

                    foreach (IntVec3 autoInputSlot in compAdvancedResourceProcessor.Process.Def.autoInputSlots) {
                        if (thing.Position + autoInputSlot.RotatedBy(thing.Rotation) == Position) {
                            return GraphicsCache.T3HopperInput;
                        }
                    }
                }
            }
        }

        return GraphicsCache.T3HopperNormal;
    }
}