using System;
using HarmonyLib;
using PipeSystem;
using RimWorld;
using Verse;

namespace MassProductionExpansion.Patches;

[HarmonyPatch(typeof(Process), "TrySpawnAtCell")]
public class Process_TrySpawnAtCell
{

    [HarmonyPrefix]
    [HarmonyInfix(typeof(GenPlace), nameof(GenPlace.TryPlaceThing),
        [
            typeof(Thing), typeof(IntVec3), typeof(Map), typeof(ThingPlaceMode), typeof(Thing), typeof(Action<Thing, int>), typeof(Predicate<IntVec3>),
            typeof(Rot4?),
            typeof(int)
        ],
        [
            ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Normal, ArgumentType.Out, ArgumentType.Normal, ArgumentType.Normal,
            ArgumentType.Normal, ArgumentType.Normal
        ])]
    static void BeforeTryPlaceThing(ref Thing thing) {

        if (thing.def.Minifiable) {
            thing = thing.TryMakeMinified();
        }
    }

}