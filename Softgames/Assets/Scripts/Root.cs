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
        public CanvasScaler CanvasScaler;
        public List<AssignmentScreen> Screens;
        private int currentIndex;
        private bool isMoving;
        void Awake()
        {
            currentIndex = StartIndex;
            Assert.IsTrue(StartIndex < Screens.Count);
            Navigation.leftPressed += () => MoveScreens(-1);
            Navigation.rightPressed += () => MoveScreens(1);
           // SetUpScreenMatch();
            SetupScreens();
        }

        private void MoveScreens(int direction)
        {
            if (isMoving)
                return;
            var width = SetupScreens();
            Navigation.ScreenName.text = "";
            isMoving = true;
            var tween = ScreensRoot.DOLocalMove(new Vector3(width * -direction, 0, 0), 0.4f).SetEase(Ease.InOutSine);
            tween.OnStepComplete(() =>
            {
                SetupNewScreenOrder(direction);
                ScreensRoot.transform.localPosition = Vector3.zero;
                isMoving = false;
            });
        }
        private int TrueModulo(int a, int b)
        {
            return ((a % b) + b) % b;
        }

        private void SetupNewScreenOrder(int direction)
        {
            currentIndex = TrueModulo(currentIndex + direction, Screens.Count);
            SetupScreens();
        }

        private float SetupScreens()
        {
            var width = (transform as RectTransform).rect.width;
            
            var left = TrueModulo(currentIndex - 1, Screens.Count);
            var right = TrueModulo(currentIndex + 1, Screens.Count);
            for (int i = 0; i < Screens.Count; i++)
            {
                Screens[i].gameObject.SetActive(i == left || i == currentIndex || i == right);
            }
            
            PutLeft(Screens[left].RectTransform, width);
            PutCenter(Screens[currentIndex].RectTransform);
            PutRight(Screens[right].RectTransform, width);

            Navigation.ScreenName.text = Screens[currentIndex].Name;
            return width;
        }

        private void SetUpScreenMatch()
        {
            if (Screen.width > Screen.height)
                CanvasScaler.matchWidthOrHeight = 1;
            else if (Screen.width < Screen.height)
                CanvasScaler.matchWidthOrHeight = 0;
            else
                CanvasScaler.matchWidthOrHeight = 0.5f;
        }

        private void PutRight(RectTransform target, float width)
        {
            target.anchorMin = new Vector2(1, 0);
            target.anchorMax = Vector2.one;
            target.pivot = new Vector2(0, 0.5f);
            target.sizeDelta = new Vector2(width, target.sizeDelta.y);
        }

        private void PutLeft(RectTransform target, float width)
        {
            target.anchorMin = Vector2.zero;
            target.anchorMax = new Vector2(0, 1);
            target.pivot = new Vector2(1, 0.5f);
            target.sizeDelta = new Vector2(width, target.sizeDelta.y);
        }
        
        private void PutCenter(RectTransform target)
        {
            target.anchorMin = Vector2.zero;
            target.anchorMax = Vector2.one;
            target.pivot = new Vector2(0.5f, 0.5f);
            target.sizeDelta = Vector2.zero;
        }

        void Update()
        {
            //SetUpScreenMatch();
        }
    }
}