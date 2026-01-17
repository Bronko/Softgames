using System;
using UnityEngine;
using UnityEngine.Serialization;

public class ParticleChangeAnimationThingy : AnimationThingy
{
    private static readonly string TriggerNext = "Next";
    
    private bool isAnimating;

    
    public void TriggerChange()
    {
        if (!isAnimating)
            SetTrigger(TriggerNext, () => isAnimating = false);
    }
}