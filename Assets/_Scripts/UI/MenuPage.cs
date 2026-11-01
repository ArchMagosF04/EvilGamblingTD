using Alchemy.Inspector;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class MenuPage : MonoBehaviour
{
    //[Tooltip("Defines the GameObject that will be selected by default when opeing this menu")]
    //[SerializeField] private Selectable objectFirstSelected;

    //private CanvasGroup canvasGroup;

    //[BoxGroup("Animation Settings"), SerializeField] private List<UIAnimatedElement> elements = new();
    //[BoxGroup("Animation Settings"), SerializeField] private float showDuration;
    //[BoxGroup("Animation Settings"), SerializeField] private float hideDuration;

    //public bool IsMenuOpen { get; private set; }

    //private Tween pageToggleTween;

    //private void Awake()
    //{
    //    canvasGroup = GetComponent<CanvasGroup>();
    //}

    //private void Start()
    //{
    //    canvasGroup.alpha = 0.0f;
    //    CloseMenu();
    //}

    //public void OpenMenu(Action OnOpenEnd = null)
    //{
    //    IsMenuOpen = true;

    //    canvasGroup.alpha = 1.0f;

    //    if (pageToggleTween != null && pageToggleTween.IsActive()) pageToggleTween.Kill(true);

    //    if (elements.Count > 0)
    //    {
    //        foreach (UIAnimatedElement element in elements)
    //        {
    //            element.ShowElement();
    //        }

    //        float timer = 0;

    //        pageToggleTween = DOTween.To(() => timer, x => timer = x, timer, hideDuration)
    //        .OnComplete(() => DelayedOpenAction(OnOpenEnd)).SetLink(gameObject).SetUpdate(true);
    //    }
    //    else
    //    {
    //        DelayedOpenAction(OnOpenEnd);
    //    }

    //    ChangeObjectSelected();
    //}

    //private void DelayedOpenAction(Action action)
    //{
    //    if (action != null) action?.Invoke();
    //    canvasGroup.interactable = true; // Enables buttons/inputs
    //    canvasGroup.blocksRaycasts = true; // Detects clicks.
    //}

    //public void ForceOpenMenu()
    //{
    //    IsMenuOpen = true;

    //    canvasGroup.alpha = 1.0f;
    //    canvasGroup.interactable = true; // Enables buttons/inputs
    //    canvasGroup.blocksRaycasts = true; // Detects clicks.

    //    ChangeObjectSelected();
    //}

    //public void CloseMenu(Action OnCloseEnd = null)
    //{
    //    IsMenuOpen = false;

    //    canvasGroup.interactable = false; // Disables buttons/inputs
    //    canvasGroup.blocksRaycasts = false; // Allows clicks to pass through to the game

    //    if (pageToggleTween != null && pageToggleTween.IsActive()) pageToggleTween.Kill(true);

    //    if (elements.Count > 0)
    //    {
    //        foreach (UIAnimatedElement element in elements)
    //        {
    //            element.HideElement();
    //        }

    //        float timer = 0;

    //        pageToggleTween = DOTween.To(() => timer, x => timer = x, timer, hideDuration)
    //        .OnComplete(() => DelayedCloseAction(OnCloseEnd)).SetLink(gameObject).SetUpdate(true);
    //    }
    //    else
    //    {
    //        DelayedCloseAction(OnCloseEnd);
    //    }
    //}

    //public void DelayedCloseAction(Action action)
    //{
    //    if (action != null) action?.Invoke();
    //    canvasGroup.alpha = 0f;
    //}

    //public void ForceCloseMenu()
    //{
    //    IsMenuOpen = false;

    //    canvasGroup.alpha = 0f;
    //    canvasGroup.interactable = false; // Disables buttons/inputs
    //    canvasGroup.blocksRaycasts = false; // Allows clicks to pass through to the game
    //}

    //public void ChangeObjectSelected()
    //{
    //    //EventSystem.current.SetSelectedGameObject(objectFirstSelected.gameObject);
    //}
}
