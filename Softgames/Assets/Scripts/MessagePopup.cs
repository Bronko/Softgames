using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MessagePopup : AnimationThingy
{
    public Button Backdrop;
    public Button OkButton;
    public TMP_Text Text;
    public CanvasGroup CanvasGroup;
    private TaskCompletionSource<bool> tcs;

    protected override void Awake()
    {
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

    public async Task Show(string text)
    {
        tcs = new TaskCompletionSource<bool>();
        gameObject.SetActive(true);
        CanvasGroup.blocksRaycasts = true;
        Text.text = text;
        await ShowAsync();
        CanvasGroup.interactable = true;
        await tcs.Task;
       
    }
}
