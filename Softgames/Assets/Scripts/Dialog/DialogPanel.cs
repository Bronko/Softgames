using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class DialogPanel : MonoBehaviour
{
    public float AvatarDistanceWall = 75;
    public float TextDistanceWall = 75;
    public float TextDistanceWallAvatarOffset = 200;
    
    public RectTransform AvatarBox;
    public RawImage AvatarImage;
    public Texture2D TextureNotLoaded;
    public TMP_Text DialogText;
    public TMP_Text NameText;
    public Button ConfirmButton;
    
    private int currentIndex;
    private MagicWords parsedDialogData;
    private AvatarDefinition currentAvatar;


    void Awake()
    {
        ConfirmButton.onClick.AddListener(OnConfirmed);
    }
    private void OnConfirmed()
    {
        currentIndex = (currentIndex + 1).TrueModulo(parsedDialogData.Dialogue.Count);
        ShowPage();
    }

    public void Display(MagicWords dialog)
    {
        parsedDialogData = dialog;
        ShowPage();
    }

    private void ShowPage()
    {
        if (currentAvatar != null)
            currentAvatar.avatarLoaded -= OnAvatarLoaded;
        
        HandleAvatarData();
        
        SetTexts();
        UpdateLayout();
    }

    private void HandleAvatarData()
    {
        if (parsedDialogData.AvatarDict.TryGetValue(parsedDialogData.Dialogue[currentIndex].Name, out var avatarDefinition))
        {
            currentAvatar = avatarDefinition;
            if (currentAvatar.Texture == null)
            {
                currentAvatar.avatarLoaded += OnAvatarLoaded;
                AvatarImage.texture = TextureNotLoaded;
            }
            else
            {
                AvatarImage.texture = currentAvatar.Texture;
            }
        }
        else
        {
            Debug.LogWarning($"No avatar definition found for {parsedDialogData.Dialogue[currentIndex].Name}");
            currentAvatar = null;
            AvatarImage.texture = TextureNotLoaded;
        }
    }
    private void SetTexts()
    {
        DialogText.text = parsedDialogData.Dialogue[currentIndex].Text;
        NameText.text = parsedDialogData.Dialogue[currentIndex].Name;
    }

    private void UpdateLayout()
    {
        var rTransA = AvatarBox;
        var rTransT = DialogText.rectTransform;
        
        // Using rect transform magic, rather than setting up two variants in the scene/prefab.
        // It's a matter of team preference, but for this, I preferred speed over readability.
        if (currentAvatar == null || currentAvatar.Position == AvatarDefinition.Positions.left)
        {
            rTransA.pivot = new Vector2(0, 1);
            rTransA.anchorMin = new Vector2(0, rTransA.anchorMin.y);
            rTransA.anchorMax = new Vector2(0, rTransA.anchorMax.y);
            rTransA.anchoredPosition = new Vector2(AvatarDistanceWall, rTransA.anchoredPosition.y);
            rTransT.offsetMin = new Vector2(TextDistanceWall + TextDistanceWallAvatarOffset, rTransT.offsetMin.y);
            rTransT.offsetMax = new Vector2(-TextDistanceWall, rTransT.offsetMax.y);
            DialogText.alignment = TextAlignmentOptions.TopLeft;
        }
        else
        {
            rTransA.pivot = new Vector2(1, 1);
            rTransA.anchorMin = new Vector2(1, rTransA.anchorMin.y);
            rTransA.anchorMax = new Vector2(1, rTransA.anchorMax.y);
            rTransA.anchoredPosition = new Vector2(-AvatarDistanceWall, rTransA.anchoredPosition.y);
            rTransT.offsetMin = new Vector2(TextDistanceWall, rTransT.offsetMin.y);
            rTransT.offsetMax = new Vector2(-TextDistanceWall - TextDistanceWallAvatarOffset, rTransT.offsetMax.y);
            DialogText.alignment = TextAlignmentOptions.TopRight;
        }
    }

    private void OnAvatarLoaded(Texture2D texture)
    {
        if (texture != null)
            AvatarImage.texture = texture;
    }
}