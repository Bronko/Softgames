using System;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This script takes a screenshot of all cards, so we only render them for one frame, while nothing is moving.
/// </summary>
public class BackgroundImageHandler : MonoBehaviour
{
    private static readonly string CardLayer = "Card";

    public event Action renderTextureResized;
    
    public RawImage RenderTextureImage;
    
    private RenderTexture bGBufferTexture;
    private int lastWidth;
    private int lastHeight;
    
    private Camera mainCam;

    private RectTransform rTrans;
    void Awake()
    {
        RenderTextureImage.enabled = true;
        rTrans = transform as RectTransform;
        mainCam = Camera.main;
        bGBufferTexture = new RenderTexture(Screen.width, Screen.height, 1, RenderTextureFormat.ARGB32);
    }

    void Update()
    {
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            lastWidth = Screen.width;
            lastHeight = Screen.height;
            bGBufferTexture.Release();
            bGBufferTexture = new RenderTexture(lastWidth, lastHeight, 1);
            RenderTextureImage.texture = bGBufferTexture;
            renderTextureResized?.Invoke();
        }
    }
    
    public void TakeStackSnapshot()
    {
        var cacheMin = rTrans.anchorMin;
        var cacheMax = rTrans.anchorMax;
        var cachePivot = rTrans.pivot;
        var cacheSize = rTrans.sizeDelta;
        var cacheIndex = rTrans.GetSiblingIndex();
        
        MoveForScreenshot(Vector2.zero, Vector2.one, new Vector2(0.5f, 0.5f), Vector2.zero, rTrans.childCount -1);
        
        mainCam.cullingMask = LayerMask.GetMask(CardLayer);
        mainCam.targetTexture = bGBufferTexture;
        
        mainCam.Render();
        
        mainCam.targetTexture = null;
        mainCam.cullingMask = -1;

        MoveForScreenshot(cacheMin, cacheMax, cachePivot, cacheSize, cacheIndex);
    }

    private void MoveForScreenshot(Vector2 anchorMin, Vector2 anchorMax, Vector2 pivot, Vector2 sizeDelta, int siblingIndex)
    {
        rTrans.anchorMin = anchorMin;
        rTrans.anchorMax = anchorMax;
        rTrans.pivot = pivot;
        rTrans.sizeDelta = sizeDelta;
        rTrans.SetSiblingIndex(siblingIndex);
    }
}