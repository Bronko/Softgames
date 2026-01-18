using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

/// <summary>
/// Example of using private SerializeField rather than public serialization.
/// Is the boiler plate worth it? It depends on the team. 
/// </summary>
public class Navigation : MonoBehaviour
{
    public event Action leftPressed;
    public event Action rightPressed;
    public event Action infoPressed;

    public TMP_Text ScreenName;
    [SerializeField] private Button left;
    [SerializeField] private Button right;
    [SerializeField] private Button info;

    void Awake()
    {
        left.onClick.AddListener(() => leftPressed?.Invoke());
        right.onClick.AddListener(() => rightPressed?.Invoke());
        info.onClick.AddListener(() => infoPressed?.Invoke());
    }
}