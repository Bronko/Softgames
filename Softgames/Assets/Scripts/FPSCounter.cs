using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    public TMP_Text FPSText;
    public float SampleTimeWindow = 0.1f;
    
    private float elapsedTime;
    private int frames;

    void Update()
    {
        elapsedTime += Time.unscaledDeltaTime;
        frames++;
        
        if (elapsedTime >= SampleTimeWindow)
        {
            var fps = frames / elapsedTime;
            frames = 0;
            elapsedTime = 0f;
            FPSText.text = $"FPS:\n{(int)fps}";
        }
    }
}