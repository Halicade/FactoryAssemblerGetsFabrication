using System.Collections.Generic;
using PipeSystem;
using RimWorld;
using Verse;

namespace MassProductionExpansion.Generator;

public static class Medicine
{
    public static ProcessDef ProcessFromMedicineRecipe(string prefix, int tickMultiplier, ProcessorTemplateDef tp,
        ThingDef def, int index, ThingDef defGettingRecipes, bool hotReload = false) {
        string defName = prefix + def.defName;
        ProcessDef processDef = (hotReload
            ? (DefDatabase<ProcessDef>.GetNamed(defName, errorOnFail: false) ?? new ProcessDef())
            : new ProcessDef());
        processDef.defName = defName;
        processDef.label = tp.label.Formatted(def.label);
        processDef.description = tp.description.Formatted(def.label);
        processDef.priorityInBillList = index;
        processDef.spawnOnInteractionCell = tp.spawnOnInteractionCell;
        processDef.autoGrabFromHoppers = tp.autoGrabFromHoppers;
        processDef.autoInputSlots = tp.autoInputSlots;
        processDef.disallowMixing = tp.disallowMixing;

        float calculatedTicks = def.GetStatValueAbstract(StatDefOf.WorkToMake);
        if (calculatedTicks <= 0) {
            calculatedTicks = 3200;
        }

        processDef.ticks = (int)(calculatedTicks * tickMultiplier);


        if (def.costList != null) {
            List<ProcessDef.Ingredient> ingredientList = [];
            foreach (var cost in def.costList) {
                ingredientList.Add(new ProcessDef.Ingredient {
                    thing = cost.thingDef,
                    countNeeded = cost.count * 4
                });
            }

            processDef.ingredients.AddRange(ingredientList);
        }

        ThingCategoryDef category = null;
        if (def.stuffCategories?.Contains(StuffCategoryDefOf.Fabric) == true) {
            category = ThingCategoryDefOf.Textiles;
        }

        if (category != null) {
            processDef.ingredients.Add(new ProcessDef.Ingredient {
                thingCategory = category,
                countNeeded = def.costStuffCount * 4
            });
        }

        processDef.results = [
            new ProcessDef.Result {
                thing = def,
                count = 4
            }
        ];
        processDef.isFactoryProcess = tp.isFactoryProcess;
        processDef.autoExtract = tp.autoExtract;
        processDef.onlyGrabAndOutputToFactoryHoppers = tp.onlyGrabAndOutputToFactoryHoppers;
        processDef.useFirstIngredientAsOutputStuff = def.costStuffCount != 0;
        processDef.sustainerWhenWorking = tp.sustainerWhenWorking;
        processDef.sustainerDef = tp.sustainerDef;
        processDef.effecterWhenWorking = tp.effecterWhenWorking;
        processDef.effecterDef = tp.effecterDef;
        processDef.maxOutputCount = tp.maxOutputCount;

        if (MassProductionExpansion.ChemfuelExpandedActive &&
            processDef.ingredients.ContainsAny(x => x.thing == ThingDefOf.Chemfuel)) {
            processDef.considerBuildingCompResource = true;
        }

        if (def.recipeMaker?.researchPrerequisites != null) {
            processDef.researchPrerequisites = [];
            processDef.researchPrerequisites.AddRange(def.recipeMaker.researchPrerequisites);
        }
        else if (def.recipeMaker?.researchPrerequisite != null) {
            processDef.researchPrerequisites = [
                def.recipeMaker.researchPrerequisite
            ];
        }

        defGettingRecipes.GetCompProperties<CompProperties_AdvancedResourceProcessor>()
            .processes.Add(processDef);
        return processDef;
    }
}