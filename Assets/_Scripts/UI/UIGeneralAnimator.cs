using Alchemy.Inspector;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UIGeneralAnimator : UIAnimatedElement
{
    [BoxGroup("Components"), SerializeField] private CanvasGroup canvasGroup;
    [BoxGroup("Components"), SerializeField] private RectTransform rectTransform;

    [TabGroup("Show Animations", "Fade"), SerializeField] private bool showFadeAnim;
    [TabGroup("Hide Animations", "Fade"), SerializeField] private bool hideFadeAnim;

    [TabGroup("Show Animations", "Scale"), SerializeField, HelpBox("Don't enable if anchors are set to strech")] private bool showScaleAnim;
    [TabGroup("Show Animations", "Scale"), SerializeField, ShowIf("showScaleAnim")] private bool showScaleIncludeX = true; //Whether to also enlarge the object on the X axis
    [TabGroup("Show Animations", "Scale"), SerializeField, ShowIf("showScaleAnim")] private bool showScaleIncludeY = true; //Whether to also enlarge the object on the Y axis
    [TabGroup("Hide Animations", "Scale"), SerializeField, HelpBox("Don't enable if anchors are set to strech")] private bool hideScaleAnim;
    [TabGroup("Hide Animations", "Scale"), SerializeField, ShowIf("hideScaleAnim")] private bool hideScaleIncludeX = true; //Whether to also shrink the object on the X axis
    [TabGroup("Hide Animations", "Scale"), SerializeField, ShowIf("hideScaleAnim")] private bool hideScaleIncludeY = true; //Whether to also shrink the object on the Y axis
    private Vector3 startingScale;
    private Vector2 startSizeDelta;

    [TabGroup("Show Animations", "Move"), SerializeField] private bool showMoveAnim;
    [TabGroup("Show Animations", "Move"), SerializeField, ShowIf("showMoveAnim"), HelpBox("Make sure this Transform is parented to the same object as the one holding this script, both with the same anchor point")] private RectTransform showMoveStartPos; //Position from where it starts the show animation.
    [TabGroup("Hide Animations", "Move"), SerializeField] private bool hideMoveAnim;
    [TabGroup("Hide Animations", "Move"), SerializeField, ShowIf("hideMoveAnim"), HelpBox("Make sure this Transform is parented to the same object as the one holding this script, both with the same anchor point")] private RectTransform hideMoveEndPos; //Position it goes towards in hide animation.
    private Vector2 startingPosition;

    private Tween fadeTween;
    private Tween scaleTween;
    private Tween moveTween;

    private void Awake()
    {
        if (rectTransform == null) rectTransform = GetComponent<RectTransform>();
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        startingScale = rectTransform.localScale;
        startSizeDelta = rectTransform.sizeDelta;
        startingPosition = rectTransform.anchoredPosition;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    public override void ShowElement()
    {
        base.ShowElement();

        ToggleInteraction(false);

        if (showFadeAnim)
        {
            canvasGroup.alpha = 0;

            if (fadeTween != null && fadeTween.IsActive()) fadeTween.Kill(true);

            fadeTween = canvasGroup.DOFade(1, showDuration).SetDelay(showDelay).SetEase(showEase).SetLink(gameObject).SetUpdate(useUnscaledTime).OnComplete(() => ToggleInteraction(true));
        }

        if (showScaleAnim)
        {
            rectTransform.localScale = new Vector3(showScaleIncludeX ? 0f : startingScale.x, showScaleIncludeY ? 0f : startingScale.y, startingScale.z);
            rectTransform.sizeDelta = startSizeDelta;

            if (scaleTween != null && scaleTween.IsActive()) scaleTween.Kill(true);

            Vector3 scaleVector = rectTransform.localScale;

            scaleTween = DOTween.To(() => scaleVector, x => scaleVector = x, startingScale, showDuration)
                .OnUpdate(() =>
                {
                    rectTransform.localScale = scaleVector;
                    rectTransform.sizeDelta = startSizeDelta;
                })
                .SetDelay(showDelay).SetEase(showEase).SetLink(gameObject).SetUpdate(useUnscaledTime).OnComplete(() => ToggleInteraction(true));
        }

        if (showMoveAnim)
        {
            rectTransform.anchoredPosition = showMoveStartPos.anchoredPosition;

            if (moveTween != null && moveTween.IsActive()) moveTween.Kill(true);

            moveTween = rectTransform.DOAnchorPos(startingPosition, showDuration).SetDelay(showDelay).SetEase(showEase).SetLink(gameObject).SetUpdate(useUnscaledTime).OnComplete(() => ToggleInteraction(true));
        }
    }

    public override void HideElement()
    {
        base.HideElement();

        ToggleInteraction(false);

        if (hideFadeAnim)
        {
            canvasGroup.alpha = 1f;

            if (fadeTween != null && fadeTween.IsActive()) fadeTween.Kill(true);

            fadeTween = canvasGroup.DOFade(0, hideDuration).SetEase(hideEase).SetDelay(hideDelay).SetLink(gameObject).SetUpdate(useUnscaledTime);
        }

        if (hideScaleAnim)
        {
            rectTransform.sizeDelta = startingScale;
            rectTransform.sizeDelta = startSizeDelta;

            if (scaleTween != null && scaleTween.IsActive()) scaleTween.Kill(true);

            Vector3 scaleVector = rectTransform.localScale;

            scaleTween = DOTween.To(() => scaleVector, x => scaleVector = x, new Vector3(hideScaleIncludeX ? 0f : startingScale.x, hideScaleIncludeY ? 0f : startingScale.y, startingScale.z), hideDuration)
                .OnUpdate(() =>
                {
                    rectTransform.localScale = scaleVector;
                    rectTransform.sizeDelta = startSizeDelta;
                })
                .SetDelay(hideDelay).SetEase(hideEase).SetLink(gameObject).SetUpdate(useUnscaledTime);
        }

        if (hideMoveAnim)
        {
            rectTransform.anchoredPosition = startingPosition;

            if (moveTween != null && moveTween.IsActive()) moveTween.Kill(true);

            moveTween = rectTransform.DOAnchorPos(hideMoveEndPos.anchoredPosition, hideDuration).SetDelay(hideDelay)
                .SetEase(hideEase).SetLink(gameObject).SetUpdate(useUnscaledTime);
        }
    }

    private void ToggleInteraction(bool toggle)
    {
        canvasGroup.interactable = toggle;
        canvasGroup.blocksRaycasts = toggle;
    }
}
