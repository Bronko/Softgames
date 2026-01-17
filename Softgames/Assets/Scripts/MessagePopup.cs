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
        Time.timeScale =
            0; //Hacky way to pause the game, and also make sure, the card stacks don't attempt to open this popup on top.
        //A real life scenario would need a better system, like a centralized pause manager or throwing if a second popup is to be opened
        //or spawning a copy on top or establishing a queue... Not in our Singleton though! ;-)
        //The cards task still pauses in a less aggressive way, even though that is redundant now,
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
        CanvasGroup.interactable = false;
        Backdrop.onClick.AddListener(Hide);
        OkButton.onClick.AddListener(Hide);
        gameObject.SetActive(false);
    }

    private async void Hide()
    {
        CanvasGroup.interactable = false;
        await HideAsync();
        gameObject.SetActive(false);
        tcs.SetResult(true);
        Time.timeScale = 1;
    }
}
