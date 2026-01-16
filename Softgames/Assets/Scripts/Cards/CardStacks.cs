using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CardsStacks : AssignmentScreen
{
    public event Action messageConfirmed;
    
    public int CardAmount = 144;
    public Transform CardPrefab;
    public Sprite CardSprite;
    public float DistanceFromCamera = 1000;
    
    public Transform StackOne;
    public Transform StackTwo;
    public float Padding = 200;
    public float RotationStep = -3;
    public float StackYStep = 2;
    
    [DoNotSerialize]
    public List<Transform> Cards = new();
    
    
    void Awake()
    {
        for (int i = 0; i < CardAmount; i++)
        {
            var card = Instantiate(CardPrefab, StackOne);
            Cards.Add(card);
            card.Rotate(Vector3.forward, RotationStep * i);
            card.transform.localPosition = new Vector3(0, StackYStep * i, -1 * i);
        }
        CardPrefab.gameObject.SetActive(false);
    }


    public void OnCardMoved(bool endReached)
    {
        Debug.Log("CardMoved");
        //Todo: Counters and message
    }
}