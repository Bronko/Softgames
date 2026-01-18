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
                                    "I guess this message is rather annoying, if you are currently looking at another task. " +
                                    "Yeah, maybe I did interpret it like this on purpose. <sprite name=\"satisfied\">\n\n" +
                                    "Also, wouldn't it look much better with the counters under the stacks?");
            messageConfirmed?.Invoke();
        }
    }
    public override string GetDescription()
    {
        return "Reading the name of the challenge again, I assume, you wanted to see me dealing with alpha blending?\n" +
               "And I tried to optimize with a shader by making it cut out and write to depth.\n" +
               "Please note, I am not a shader specialist. <sprite name=\"laughing\">. So, well, it didn't work. I guess, batching and depth sorting together can't happen? \n" +
               "In a real life scenario I would have consulted my peers for this.\n" +
               "\n" +
               "Maybe some provided assets would have made things more clear. After all my first idea was to use a repeating filling of a 9-slice asset to simulate the stack, which I was told, is not the idea.\n"+
               "Well, now we will stick with the \"cutout\" sprite."+
               "\n" +
               "Anyway, possible improvement: More steps in the animation, or creating a wrapper to allow different eases on Y and X, to make the cards travel a" +
               "smooth curve.\n";
    }
}