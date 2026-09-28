using Alchemy.Inspector;
using DG.Tweening;
using UnityEngine;

public class UIAnimatedElement : MonoBehaviour
{
    [SerializeField] protected bool useUnscaledTime = true;

    [TabGroup("Animation Settings", "Show"), SerializeField] protected float showDuration;
    [TabGroup("Animation Settings", "Show"), SerializeField] protected float showDelay;
    [TabGroup("Animation Settings", "Show"), SerializeField] protected Ease showEase;

    [TabGroup("Animation Settings", "Hide"), SerializeField] protected float hideDuration;
    [TabGroup("Animation Settings", "Hide"), SerializeField] protected float hideDelay;
    [TabGroup("Animation Settings", "Hide"), SerializeField] protected Ease hideEase;

    public virtual void ShowElement()
    {

    }

    public virtual void HideElement()
    {

    }
}
