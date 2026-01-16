using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    public MessagePopup Message;
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


    public async void OnCardMoved(bool endReached)
    {
        Debug.Log("CardMoved");
        //Todo: Counters

        if (endReached)
        {
            await Message.Show("All the ladybugs have travelled to their other stack!\n\n" +
                               " I guess this message is rather annoying, if you are currently looking at another task, but I believe this is fair after the amount of context switches I had to go through for all of this.");
            messageConfirmed?.Invoke();
        }
    }
}