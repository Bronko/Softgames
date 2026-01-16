using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
    public float AvatarDistanceWall = 75;
    public float TextDistanceWall = 60;
    public float TextDistanceWallAvatarOffset = 200;
    
    public RectTransform AvatarBox;
    public RawImage AvatarImage;
    public Texture2D NotLoaded;
    public TMP_Text Text;
    public TMP_Text NameText;
    
    private int currentIndex = 0;
    private MagicWords parsedDialog;
    private AvatarDefinition currentAvatar;
    
    public Button ConfirmButton;

    void Awake()
    {
        ConfirmButton.onClick.AddListener(OnConfirmed);
    }
    private void OnConfirmed()
    {
        currentIndex = (currentIndex + 1) % parsedDialog.dialogue.Count;
        ShowPage();
    }

    public void Display(MagicWords dialog)
    {
        parsedDialog = dialog;
        ShowPage();
    }

    private void ShowPage()
    {
        if (currentAvatar != null)
            currentAvatar.avatarLoaded -= OnAvatarLoaded;
        if (parsedDialog.Avatars.TryGetValue(parsedDialog.dialogue[currentIndex].name, out var avatarDefinition))
        {
            currentAvatar = avatarDefinition;
            if (currentAvatar.Texture == null)
            {
                currentAvatar.avatarLoaded += OnAvatarLoaded;
                AvatarImage.texture = NotLoaded;
            }
            else
            {
                AvatarImage.texture = currentAvatar.Texture;
            }
        }
        else
        {
            Debug.LogWarning($"No avatar definition found for {parsedDialog.dialogue[currentIndex].name}");
            currentAvatar = null;
            AvatarImage.texture = NotLoaded;
        }
        
        Text.text = parsedDialog.dialogue[currentIndex].text;
        NameText.text = parsedDialog.dialogue[currentIndex].name;
        UpdateLayout();
    }

    private void UpdateLayout()
    {
        var rTransA = AvatarBox;
        var rTransT = Text.rectTransform;
        if (currentAvatar == null || currentAvatar.position == AvatarDefinition.Position.left)
        {
            rTransA.pivot = new Vector2(0, 1);
            rTransA.anchorMin = new Vector2(0, rTransA.anchorMin.y);
            rTransA.anchorMax = new Vector2(0, rTransA.anchorMax.y);
            rTransA.anchoredPosition = new Vector2(AvatarDistanceWall, rTransA.anchoredPosition.y);
            rTransT.offsetMin = new Vector2(TextDistanceWall + TextDistanceWallAvatarOffset, rTransT.offsetMin.y);
            rTransT.offsetMax = new Vector2(-TextDistanceWall, rTransT.offsetMax.y);
            Text.alignment = TextAlignmentOptions.TopLeft;
        }
        else
        {
            rTransA.pivot = new Vector2(1, 1);
            rTransA.anchorMin = new Vector2(1, rTransA.anchorMin.y);
            rTransA.anchorMax = new Vector2(1, rTransA.anchorMax.y);
            rTransA.anchoredPosition = new Vector2(-AvatarDistanceWall, rTransA.anchoredPosition.y);
            rTransT.offsetMin = new Vector2(TextDistanceWall, rTransT.offsetMin.y);
            rTransT.offsetMax = new Vector2(-TextDistanceWall - TextDistanceWallAvatarOffset, rTransT.offsetMax.y);
            Text.alignment = TextAlignmentOptions.TopRight;
        }
    }

    private void OnAvatarLoaded(Texture2D texture)
    {
        if (texture != null)
            AvatarImage.texture = texture;
    }
}