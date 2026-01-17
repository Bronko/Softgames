using System.Collections.Generic;
using DG.Tweening;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Root : MonoBehaviour
    {
        public Navigation Navigation;
        public RectTransform ScreensRoot;
        public int StartIndex = 1;
        public List<AssignmentScreen> Screens;
        private int currentIndex;
        private bool isMoving;
        AssignmentScreen Left => Screens[(currentIndex - 1).TrueModulo(Screens.Count)]; 
        AssignmentScreen Right => Screens[(currentIndex + 1).TrueModulo(Screens.Count)];
        AssignmentScreen Current => Screens[(currentIndex).TrueModulo(Screens.Count)];
        void Awake()
        {
            currentIndex = StartIndex;
            Assert.IsTrue(StartIndex < Screens.Count);
            Navigation.leftPressed += () => MoveScreens(-1);
            Navigation.rightPressed += () => MoveScreens(1);
            SetupScreens();
        }

        private void MoveScreens(int direction)
        {
            if (isMoving)
                return;
            isMoving = true;
            
            var width = (transform as RectTransform).rect.width;
            var nextIndex = (currentIndex + direction).TrueModulo(Screens.Count);
            var target = Screens[nextIndex];
            
            PrepareTarget(target, width);
            Navigation.ScreenName.text = "";
          
            var targetPosition = new Vector3(width * -direction, 0, 0);
            var tween = ScreensRoot.DOLocalMove(targetPosition, 0.4f).SetEase(Ease.InOutSine);
            tween.OnStepComplete(() =>
            {
                currentIndex = nextIndex;
                SetupScreens();
                ScreensRoot.transform.localPosition = Vector3.zero;
                isMoving = false;
            });
        }

        private void PrepareTarget(AssignmentScreen target, float width)
        {
            target.gameObject.SetActive(true);
            target.RectTransform.sizeDelta = new Vector2(width, target.RectTransform.sizeDelta.y);
        }

        private void SetupScreens()
        {
            for (int i = 0; i < Screens.Count; i++)
            {
                Screens[i].gameObject.SetActive(i == currentIndex);
            }
            
            PutLeft(Left.RectTransform);
            PutCenter(Current.RectTransform);
            PutRight(Right.RectTransform);

            Navigation.ScreenName.text = Screens[currentIndex].Name;
        }
        
        private void PutLeft(RectTransform target)
        {
            target.anchorMin = Vector2.zero;
            target.anchorMax = new Vector2(0, 1);
            target.pivot = new Vector2(1, 0.5f);
        }
        private void PutRight(RectTransform target)
        {
            target.anchorMin = new Vector2(1, 0);
            target.anchorMax = Vector2.one;
            target.pivot = new Vector2(0, 0.5f);
        }
        
        private void PutCenter(RectTransform target)
        {
            target.anchorMin = Vector2.zero;
            target.anchorMax = Vector2.one;
            target.pivot = new Vector2(0.5f, 0.5f);
            target.sizeDelta = Vector2.zero;
        }
        
    }
}