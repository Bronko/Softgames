using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Root : MonoBehaviour
    {
        [SerializeField]
        private Navigation navigation;
        
        [SerializeField]
        private int startIndex;
        
        [SerializeField]
        private CanvasScaler canvasScaler;
        
        [SerializeField]
        private List<AssignmentScreen> Screens;

        void Awake()
        {
            Assert.IsTrue(startIndex < Screens.Count);
            SetUpScreenMatch();
            SetupScreens();
        }

        private void SetupScreens()
        {
            for (int i = 0; i < Screens.Count; i++)
            {
                Screens[i].gameObject.SetActive(i >= startIndex - 1 && i <= startIndex + 1);
            }
            
            PutLeft(Screens[startIndex - 1].RectTransform);
            PutCenter(Screens[startIndex].RectTransform);
            PutRight(Screens[startIndex + 1].RectTransform);

            navigation.ScreenName.text = Screens[startIndex].Name;
        }

        private void SetUpScreenMatch()
        {
            if (Screen.width > Screen.height)
                canvasScaler.matchWidthOrHeight = 1;
            else if (Screen.width < Screen.height)
                canvasScaler.matchWidthOrHeight = 0;
            else
                canvasScaler.matchWidthOrHeight = 0.5f;
        }

        private void PutRight(RectTransform target)
        {
            target.anchorMin = new Vector2(1, 0);
            target.anchorMax = Vector2.one;
            target.pivot = new Vector2(0, 0.5f);
            target.sizeDelta = new Vector2(canvasScaler.referenceResolution.x, target.sizeDelta.y);
        }

        private void PutLeft(RectTransform target)
        {
            target.anchorMin = Vector2.zero;
            target.anchorMax = new Vector2(0, 1);
            target.pivot = new Vector2(1, 0.5f);
            target.sizeDelta = new Vector2(canvasScaler.referenceResolution.x, target.sizeDelta.y);
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
            SetUpScreenMatch();
        }
    }
}