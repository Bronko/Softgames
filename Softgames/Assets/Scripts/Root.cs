using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Root : MonoBehaviour
{
    public Navigation Navigation;
    public RectTransform ScreensRoot;
    public List<AssignmentScreen> Screens;

    [Tooltip(
        "These objects will be set active. To not conflict with self deactivating objects, Script execution order of root is forced earlier")]
    public List<GameObject> ForceActiveObjects;

    [Tooltip("The screen to show first")] public int StartIndex = 1;

    private int currentIndex;
    private bool isMoving;

    AssignmentScreen Left => Screens[(currentIndex - 1).TrueModulo(Screens.Count)];
    AssignmentScreen Right => Screens[(currentIndex + 1).TrueModulo(Screens.Count)];
    AssignmentScreen Current => Screens[(currentIndex).TrueModulo(Screens.Count)];

    void Awake()
    {
        ActivateForceActiveObjects();

        if (StartIndex >= Screens.Count || StartIndex < 0)
            Debug.LogError("StartIndex out of range");

        currentIndex = StartIndex;

        Navigation.leftPressed += () => MoveScreens(-1);
        Navigation.rightPressed += () => MoveScreens(1);
        Navigation.infoPressed += ShowScreenInfo;
        SetupScreens();
    }

    private void ActivateForceActiveObjects()
    {
        foreach (var obj in ForceActiveObjects)
        {
            if (!obj.activeSelf)
                obj.SetActive(true);
        }
    }

    private void ShowScreenInfo()
    {
        var text = Current.GetDescription();

        MessagePopup.Show(text);
    }

    private void MoveScreens(int direction)
    {
        if (isMoving)
            return;
        isMoving = true;

        var width = (transform as RectTransform).rect.width;
        var nextIndex = (currentIndex + direction).TrueModulo(Screens.Count);
        var target = Screens[nextIndex];

        SetSideScreenWidth(target.RectTransform,
            width); //For if the aspect ratio changed we read it only before moving.
        //A bit overkill I guess. But it helped debugging.
        Navigation.ScreenName.text = "";

        var targetPosition = new Vector3(width * -direction, 0, 0);
        var tween = ScreensRoot.DOLocalMove(targetPosition, 0.4f).SetEase(Ease.InOutSine);
        tween.OnStepComplete(() =>
        {
            currentIndex = nextIndex;
            SetupScreens();
            ScreensRoot.transform.localPosition = Vector3.zero;
            isMoving = false;
        });
    }

    private void SetSideScreenWidth(RectTransform target, float width)
    {
        target.sizeDelta = new Vector2(width, target.sizeDelta.y);
    }

    private void SetupScreens()
    {
        PutLeft(Left.RectTransform);
        PutCenter(Current.RectTransform);
        PutRight(Right.RectTransform);

        Navigation.ScreenName.text = Screens[currentIndex].Name;
    }

    /// <summary>
    /// Using rect transform anchors and pivot to attach the screen to the left of the frustum
    /// </summary>
    private void PutLeft(RectTransform target)
    {
        target.anchorMin = Vector2.zero;
        target.anchorMax = new Vector2(0, 1);
        target.pivot = new Vector2(1, 0.5f);
        SetSideScreenWidth(target, 10000);
    }

    //Give it some arbitrary width, so things are not overlapping in.

    /// <summary>
    /// Using rect transform anchors and pivot to attach the screen to the right of the frustum
    /// </summary>
    private void PutRight(RectTransform target)
    {
        target.anchorMin = new Vector2(1, 0);
        target.anchorMax = Vector2.one;
        target.pivot = new Vector2(0, 0.5f);
        SetSideScreenWidth(target, 10000);
    }

    /// <summary>
    /// Setting the rect transform anchors and pivot to stretch to the full parent, in this case, the whole screen.
    /// </summary>
    private void PutCenter(RectTransform target)
    {
        target.anchorMin = Vector2.zero;
        target.anchorMax = Vector2.one;
        target.pivot = new Vector2(0.5f, 0.5f);
        target.sizeDelta = Vector2.zero;
    }
}
