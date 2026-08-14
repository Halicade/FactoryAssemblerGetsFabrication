using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace MassProductionExpansion.Commands;

[StaticConstructorOnStartup]
public class SetToGrowSmart : Command
{
    public IPlantToGrowSettable smartSettable;

    private List<IPlantToGrowSettable> smartSettabbles;

    private static readonly List<ThingDef> tmpAvailablePlants = [];

    private static readonly Texture2D SetPlantToGrowTex = ContentFinder<Texture2D>.Get("UI/Commands/SetPlantToGrow");

    public SetToGrowSmart() {
        tutorTag = "GrowingZoneSetPlant";
        ThingDef thingDef = null;
        bool flag = false;
        foreach (object selectedObject in Find.Selector.SelectedObjects) {
            if (selectedObject is IPlantToGrowSettable plantToGrowSettable) {
                if (thingDef != null && thingDef != plantToGrowSettable.GetPlantDefToGrow()) {
                    flag = true;
                    break;
                }

                thingDef = plantToGrowSettable.GetPlantDefToGrow();
            }
        }

        if (flag) {
            icon = SetPlantToGrowTex;
            defaultLabel = "CommandSelectPlantToGrowMulti".Translate();
            return;
        }

        if (thingDef == null) {
            icon = SetPlantToGrowTex;
            defaultLabel = "CommandSelectPlantToGrow".Translate("None".Translate().CapitalizeFirst());
        }
        else {
            icon = thingDef.uiIcon;
            iconAngle = thingDef.uiIconAngle;
            iconOffset = thingDef.uiIconOffset;
            defaultLabel = "CommandSelectPlantToGrow".Translate(thingDef.LabelCap);
        }
    }

    
    public override void ProcessInput(Event ev) {
        base.ProcessInput(ev);
        List<FloatMenuOption> list = new List<FloatMenuOption>();
        if (smartSettabbles == null) {
            smartSettabbles = new List<IPlantToGrowSettable>();
        }

        if (!smartSettabbles.Contains(smartSettable)) {
            smartSettabbles.Add(smartSettable);
        }

        tmpAvailablePlants.Clear();
        foreach (ThingDef item in PlantUtility.ValidPlantTypesForGrowers(smartSettabbles)) {
            //I had to copy this whole thing just to change a single value
            if (IsPlantAvailable(item, smartSettable.Map) && item.plant.sowMinSkill <= 12) {
                tmpAvailablePlants.Add(item);
            }
        }

        tmpAvailablePlants.SortBy(x => 0f - GetPlantListPriority(x), x => x.label);
        for (int i = 0; i < tmpAvailablePlants.Count; i++) {
            ThingDef plantDef = tmpAvailablePlants[i];
            string text = plantDef.LabelCap;
            list.Add(new FloatMenuOption(text, delegate {
                    string text2 = tutorTag + "-" + plantDef.defName;
                    if (TutorSystem.AllowAction(text2)) {
                        bool flag = true;
                        for (int j = 0; j < smartSettabbles.Count; j++) {
                            smartSettabbles[j].SetPlantDefToGrow(plantDef);
                            if (flag && plantDef.plant.interferesWithRoof) {
                                foreach (IntVec3 cell in smartSettabbles[j].Cells) {
                                    if (cell.Roofed(smartSettabbles[j].Map)) {
                                        Messages.Message(
                                            "MessagePlantIncompatibleWithRoof".Translate(
                                                Find.ActiveLanguageWorker.Pluralize(plantDef.LabelCap)),
                                            MessageTypeDefOf.CautionInput, historical: false);
                                        flag = false;
                                        break;
                                    }
                                }
                            }
                        }

                        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.SetGrowingZonePlant,
                            KnowledgeAmount.Total);
                        WarnAsAppropriate(plantDef);
                        TutorSystem.Notify_Event(text2);
                    }
                }, plantDef, null, forceBasicStyle: false, MenuOptionPriority.Default, null, null, 29f,
                rect => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, plantDef)));
        }

        if (list.Any()) {
            Find.WindowStack.Add(new FloatMenu(list));
        }
    }

    public override bool InheritInteractionsFrom(Gizmo other) {
        if (smartSettabbles == null) {
            smartSettabbles = [];
        }
        //And to prevent this from erroring. It still errors with base game and the VE factory factory but oh well
        smartSettabbles.Add((other as SetToGrowSmart)?.smartSettable);
        return false;
    }

    private void WarnAsAppropriate(ThingDef plantDef) {
        if (!plantDef.plant.diesToLight && !plantDef.plant.cavePlant) {
            return;
        }

        IntVec3 cell = IntVec3.Invalid;
        bool flag = !smartSettable.Map.GameConditionManager.IsAlwaysDarkOutside;
        for (int i = 0; i < smartSettabbles.Count; i++) {
            foreach (IntVec3 cell2 in smartSettabbles[i].Cells) {
                bool num = !flag || cell2.Roofed(smartSettabbles[i].Map);
                bool flag2 = smartSettabbles[i].Map.glowGrid.GroundGlowAt(cell2, ignoreCavePlants: true) <= 0f;
                if (!num || !flag2) {
                    cell = cell2;
                    break;
                }
            }

            if (cell.IsValid) {
                break;
            }
        }

        if (cell.IsValid) {
            Messages.Message("MessageWarningCavePlantsExposedToLight".Translate(plantDef.LabelCap),
                new TargetInfo(cell, smartSettable.Map), MessageTypeDefOf.RejectInput);
        }
    }

    public static bool IsPlantAvailable(ThingDef plantDef, Map map) {
        //Okay I also had to prevent trees
        if (plantDef.plant.IsTree || plantDef.plant.blockAdjacentSow || !plantDef.plant.treeLoversCareIfChopped) {
            return false;
        }
        List<ResearchProjectDef> sowResearchPrerequisites = plantDef.plant.sowResearchPrerequisites;
        if (sowResearchPrerequisites == null) {
            return true;
        }

        for (int i = 0; i < sowResearchPrerequisites.Count; i++) {
            if (!sowResearchPrerequisites[i].IsFinished) {
                return false;
            }
        }

        if (plantDef.plant.mustBePermanentDarknessToSow && !map.gameConditionManager.IsAlwaysDarkOutside) {
            return false;
        }

        if (plantDef.plant.mustBeWildToSow && !map.wildPlantSpawner.AllWildPlants.Contains(plantDef)) {
            return false;
        }

        return true;
    }

    private float GetPlantListPriority(ThingDef plantDef) {
        if (plantDef.plant.IsTree) {
            return 1f;
        }

        return plantDef.plant.purpose switch {
            PlantPurpose.Food => 4f,
            PlantPurpose.Health => 3f,
            PlantPurpose.Beauty => 2f,
            PlantPurpose.Misc => 0f,
            _ => 0f,
        };
    }
}