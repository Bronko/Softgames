using System;
using UnityEngine;
using UnityEngine.UI;


public class Particles : AssignmentScreen
{
    public Button ChangeButton;
    public ParticleChangeAnimationThingy ChangeAnimation;
    private void Awake()
    {
        ChangeButton.onClick.AddListener(ChangeAnimation.TriggerChange);
    }
    public override string GetDescription()
    {
        return "So I will touch a lot of tech art? Cool!\n" +
               "\n" +
               "Always fun, how particle systems interact with the canvas, eh?\n" +
               "Also fun, how some values cannot be touched by the animator.";
    }
}
