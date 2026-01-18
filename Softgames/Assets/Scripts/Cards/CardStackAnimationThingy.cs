using System;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using Unity.VisualScripting;
using UnityEngine;

public class CardStackAnimationThingy : AnimationThingy
{
    private static readonly string CardTrigger = "Go";
    
    public float SequenceStepTime;
    
    public CardsStacks CardStacks;
    [Tooltip("The position, each card travels up to, before moving to the other stack")]
    public Transform MoveUpTarget;
    public BackgroundImageHandler BackgroundImageHandler;
    
    private Transform fromStack;
    private Transform toStack;

    private float timePassed;
    private bool isPaused;
    private int iterationDirection = -1;
    private int index;
    
    private Transform currentMovingCard;
    protected override void Awake()
    {
        base.Awake();

        BackgroundImageHandler.renderTextureResized += TakeStackSnapshot;
        
        CardStacks.messageConfirmed += () => isPaused = false;
        fromStack = CardStacks.StackOne;
        toStack = CardStacks.StackTwo;
        index = CardStacks.Cards.Count - 1;
    }
    
    bool IsEndIndex()
    {
        return (iterationDirection < 0 && index <= 0) 
               || (iterationDirection > 0) && index >= CardStacks.Cards.Count - 1;
    }
    
    void Update()
    {
        if (isPaused)
            return;
        
        timePassed += Time.deltaTime;
        
        if (timePassed >= 1)
        {
            timePassed = 0;
            isPaused = true;
            var isEndIndex = IsEndIndex();
            SetTrigger(CardTrigger, () =>
            {
                isPaused = isEndIndex;
                CardStacks.OnCardMoved(isEndIndex);
            });
            if (isEndIndex)
                SwapTargets();
            else
                index += iterationDirection;
        }
    }
    private void SwapTargets()
    {
        iterationDirection *= -1;
        (toStack, fromStack) = (fromStack,toStack);
    }

    protected override void HandleTrigger(string name, Action callback)
    {
        if (name == CardTrigger)
        {
            currentMovingCard = CardStacks.Cards[index];
            currentMovingCard.transform.SetParent(toStack, true);
            TakeStackSnapshot();
            currentMovingCard.gameObject.SetActive(true);
            var seq = DOTween.Sequence();
            var targetLayer = iterationDirection < 0 ? (CardStacks.Cards.Count - 1 - index) : index;
            var endTargetZ = -targetLayer;
            var targetPos = new Vector3(0, CardStacks.StackYStep * targetLayer, -200);
            var rotateByEulers = new Vector3(0, 0, CardStacks.RotationStep * iterationDirection);
            var startLocalPos = currentMovingCard.localPosition;
            
            seq.Append(currentMovingCard.DOBlendableLocalMoveBy(toStack.InverseTransformPoint(MoveUpTarget.position) - startLocalPos, SequenceStepTime)).SetEase(Ease.InOutSine);
            seq.Join(currentMovingCard.DOScale(2f, SequenceStepTime)).SetEase(Ease.InOutSine);
            
            seq.Append(currentMovingCard.DOBlendableLocalMoveBy(targetPos - toStack.InverseTransformPoint(MoveUpTarget.position), SequenceStepTime)).SetEase(Ease.InOutSine);
            seq.Join(currentMovingCard.DOScale(1.0f, SequenceStepTime)).SetEase(Ease.InSine);
            seq.Join(currentMovingCard.DOLocalRotate(rotateByEulers, SequenceStepTime,  RotateMode.LocalAxisAdd)).SetEase(Ease.InSine);
            
            seq.AppendCallback(() =>
            {
                currentMovingCard.localPosition = new Vector3(targetPos.x, targetPos.y, endTargetZ);
                currentMovingCard = null;
                TakeStackSnapshot();
            });
            
            AddTween(seq, callback);
        }
    }

    private void TakeStackSnapshot()
    {
        foreach (var card in CardStacks.Cards)
        {
            card.gameObject.SetActive(card != currentMovingCard);
        }
        BackgroundImageHandler.TakeStackSnapshot();
        foreach (var card in CardStacks.Cards)
        {
            card.gameObject.SetActive(card == currentMovingCard);
        }
    }
}