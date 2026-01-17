using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Usually popups are bound to a central popup system, which is embedded in a general architecture.
/// As this is a simple assignment, I simulate this with a simple singleton. All the threats to complex architecture
/// don't matter in this context. So the sleekness and understandability really shine.
/// </summary>
public class MessagePopup : AnimationThingy
{
    private static MessagePopup Instance;
    
    public Button Backdrop;
    public Button OkButton;
    public TMP_Text Text;
    public CanvasGroup CanvasGroup;
    private TaskCompletionSource<bool> tcs;

    public static Task Show(string text)
    {
        return Instance.ShowInternal(text);
    }
    
    private async Task ShowInternal(string text)
    {
        tcs = new TaskCompletionSource<bool>();
        gameObject.SetActive(true);
        CanvasGroup.blocksRaycasts = true;
        Text.text = text;
        await ShowAsync();
        CanvasGroup.interactable = true;
        await tcs.Task;
    }
    protected override void Awake()
    {
        Instance = this;
        
        base.Awake();
        CanvasGroup.blocksRaycasts = false;
        CanvasGroup.interactable = false;
        Backdrop.onClick.AddListener(Hide);
        OkButton.onClick.AddListener(Hide);
        gameObject.SetActive(false);
    }
    private async void Hide()
    {
        CanvasGroup.interactable = false;
        await HideAsync();
        CanvasGroup.blocksRaycasts = false;
        gameObject.SetActive(false);
        tcs.SetResult(true);
    }
}
