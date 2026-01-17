using System;
using UnityEngine;
using UnityEngine.UI;


public class Particles : AssignmentScreen
{
    [SerializeField] private ParticleSystem particleSystem;

    public Button ChangeButton;
    public ParticleChangeAnimationThingy ChangeAnimation;
    private void Awake()
    {
        ChangeButton.onClick.AddListener(ChangeAnimation.TriggerChange);
    }
}
