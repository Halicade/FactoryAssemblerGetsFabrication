using System;
using System.Collections.Generic;
using MassProductionExpansion.ModExtension;
using PipeSystem;
using RimWorld;
using Verse;

namespace MassProductionExpansion.ResultWorker;

public class Meals : ProcessDef.ResultWorker
{
    public override int GetCount(Process process) {
        List<CachedIngredient> nutritionItems = process.advancedProcessor.cachedIngredients;

        float resultingTotalNutrition = 0f;

        foreach (CachedIngredient ingredient in nutritionItems) {
            resultingTotalNutrition +=
                ingredient.thingDef.GetStatValueAbstract(StatDefOf.Nutrition)
                * ingredient.count;
        }

        var modExtension = process.Def.GetModExtension<nutritionChanger>();

        if (modExtension == null || resultingTotalNutrition == 0) {
            return result.count;
        }
        float nutritionTotal = modExtension.nutritionToMake;
        return Math.Max((int)Math.Round(resultingTotalNutrition * result.count / nutritionTotal), 1);

    }
    
    
}