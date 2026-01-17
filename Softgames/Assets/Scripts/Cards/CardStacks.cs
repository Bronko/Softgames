using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CardsStacks : AssignmentScreen
{
    public event Action messageConfirmed;
    
    public int CardAmount = 144;
    public Transform CardPrefab;
    
    public Transform StackOne;
    public Transform StackTwo;
    
    public TMP_Text Counter1;
    public TMP_Text Counter2;
    
    [Tooltip("Amount each card gets rotated, when stacked")]
    public float RotationStep = -3;
    [Tooltip("Amount each card gets moved \"up\" when stacked")]
    public float StackYStep = 2;

    [NonSerialized]
    public List<Transform> Cards = new();
    
    
    void Awake()
    {
        Counter1.text = CardAmount.ToString();
        Counter2.text = 0.ToString();
        
        InstantiateCards();
        
        CardPrefab.gameObject.SetActive(false);
    }

    private void InstantiateCards()
    {
        for (int i = 0; i < CardAmount; i++)
        {
            var card = Instantiate(CardPrefab, StackOne);
            Cards.Add(card);
            card.Rotate(Vector3.forward, RotationStep * i);
            card.transform.localPosition = new Vector3(0, StackYStep * i, -1 * i);
        }
    }


    public async void OnCardMoved(bool endReached)
    {
        Counter1.text = StackOne.childCount.ToString();
        Counter2.text = StackTwo.childCount.ToString();
        
        if (endReached)
        {
            await MessagePopup.Show("All the ladybugs have travelled to their other stack!\n\n" +
                               "I guess this message is rather annoying, if you are currently looking at another task." +
                               "Well, maybe I have been interpreting the task on purpose like this, to pay back for all the context switching you made me go through. <sprite name=\"satisfied\">\n" +
                               "\n" +
                               "Also, wouldn't it look much gbetter with the counter under the stacks?");
            messageConfirmed?.Invoke();
        }
    }
    public override string GetDescription()
    {
        return "Reading the name of the challenge again, I assume, you wanted to see me dealing with alpha blending?\n" +
               "Sorry. This is too late now. I optimized the shader by making it cut out, and please note, I am not a shader specialist.\n" +
               "In a real life scenario I would have consulted my peers.\n" +
               "\n" +
               "Honestly, I would have liked some provided assets, to save some time and headaches, while providing more clarity.\n"+
               "\n" +
               "Possible improvement: More steps in the animation, or creating a wrapper to allow different eases on Y and X, to make the cards travel a" +
               "smoot curve. \n";
    }
}