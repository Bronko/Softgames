using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class Navigation : MonoBehaviour
    {
        public event Action leftPressed;
        public event Action rightPressed;
        public TMP_Text ScreenName;
        [SerializeField] private Button left;
        [SerializeField] private Button right;

        void Awake()
        {
            left.onClick.AddListener(() => leftPressed?.Invoke());
            right.onClick.AddListener(() => rightPressed?.Invoke());
        }
    }
}