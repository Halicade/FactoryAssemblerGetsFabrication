using System.Collections.Generic;
using Verse;

namespace MassProductionExpansion;

public class ProcessorTemplateDef : Def
{
    public bool spawnOnInteractionCell;
    public bool autoGrabFromHoppers;
    public List<IntVec3> autoInputSlots;
    public bool isFactoryProcess;
    public bool autoExtract;
    public bool onlyGrabAndOutputToFactoryHoppers;
    public bool disallowMixing;
    public bool sustainerWhenWorking;
    public SoundDef sustainerDef;
    public bool effecterWhenWorking;
    public EffecterDef effecterDef;
    public float maxOutputCount;
}
