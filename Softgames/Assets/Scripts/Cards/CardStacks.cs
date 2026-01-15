using System.Collections.Generic;
using UnityEngine;
using Screen = Unity.Android.Gradle.Manifest.Screen;

public class CardsStacks : AssignmentScreen
{
    public Transform CardPrefab;
    public Sprite CardSprite;
    public float DistanceFromCamera = 1000;
    
    public Transform StackOne;
    public Transform StackTwo;
    public float Padding = 200;
    private List<Transform> Cards = new();
    
   
    
    void Awake()
    {
        for (int i = 0; i < 144; i++)
        {
            var card = Instantiate(CardPrefab, StackOne);
            Cards.Add(card);
            card.Rotate(Vector3.forward, -1 * i);
            card.transform.localPosition = new Vector3(0, 2 * i, -1 * i);
        }
        CardPrefab.gameObject.SetActive(false);
    }

   
}