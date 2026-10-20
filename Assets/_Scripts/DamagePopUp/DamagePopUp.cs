using Alchemy.Inspector;
using DG.Tweening;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private TMP_Text damageText;

    [BoxGroup("Settings"), SerializeField, ReadOnly] private float dmgNumber;
    [BoxGroup("Settings"), SerializeField] private float lifeTime;


    [BoxGroup("Positioning"), SerializeField, Range(0f, 1f)] private float randomOffsetX;
    [BoxGroup("Positioning"), SerializeField, Range (0f, 1f)] private float randomOffsetY;
    [BoxGroup("Positioning"), SerializeField, Range(0f, 1f)] private float displacementX;
    [BoxGroup("Positioning"), SerializeField, Range(0f, 1f)] private float displacementY;
    [BoxGroup("Positioning"), SerializeField] private Ease displacementEase;

    [BoxGroup("Fade Settings"), SerializeField] private float fadeInDuration = 0.1f;
    [BoxGroup("Fade Settings"), SerializeField] private float fadeOutDuration = 0.2f;
    [BoxGroup("Fade Settings"), SerializeField] private Ease fadeInEase;
    [BoxGroup("Fade Settings"), SerializeField] private Ease fadeOutEase;

    [FoldoutGroup("TextColor"), SerializeField] private Color normalColor;
    [FoldoutGroup("TextColor"), SerializeField] private Color playerColor;


    private Color fadeColor;

    private Tween fadeTween;
    private Tween moveTween;

    private bool fading;

    private bool returned;

    private void Awake()
    {
        if (damageText == null) damageText = GetComponent<TMP_Text>();

        SetTextAlpha(0);
    }

    private void Start()
    {
        transform.DOMove(new Vector2(transform.position.x, transform.position.y + 0.7f), 0.5f, false);
    }

    private void OnEnable()
    {
        returned = false;
    }

    public void ReturnObjectToPool()
    {
        if (DamageTextPool.Instance != null)
        {
            if (!returned)
            {
                DamageTextPool.Instance.ReturnToPool(this);
                returned = true;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void InitialzeDamageText(float dmgAmount = 0, DamageTextColor color = DamageTextColor.Normal)
    {
        dmgNumber = dmgAmount;
        damageText.text = dmgNumber.ToString("F0");

        ChangeColor(color);

        transform.position += new Vector3(Random.Range(-randomOffsetX, randomOffsetX), Random.Range(0, randomOffsetY), 0);

        fading = false;

        if (moveTween != null && moveTween.IsActive()) moveTween.Kill();

        fadeTween = damageText.DOFade(1, fadeInDuration).SetLink(gameObject).SetEase(fadeInEase);

        Vector3 displacement = new Vector3(Random.Range(-displacementX, displacementX),
                                           Random.Range(displacementY * 0.75f, displacementY));

        moveTween = transform.DOLocalMove(displacement, lifeTime).SetLink(gameObject).SetEase(displacementEase).OnComplete(StartToFadeOut);
    }

    private void StartToFadeOut()
    {
        if (fading) return;

        if (fadeTween != null && fadeTween.IsActive()) fadeTween.Kill(true);

        fading = true;

        fadeTween = damageText.DOFade(0, fadeOutDuration).SetLink(gameObject).SetEase(fadeOutEase).OnComplete(ReturnObjectToPool);
    }

    private void OnDisable()
    {
        if (moveTween != null && moveTween.IsActive()) moveTween.Kill();

        SetTextAlpha(0);
    }

    private void SetTextAlpha(float alpha)
    {
        fadeColor = damageText.color;
        fadeColor.a = alpha;
        damageText.color = fadeColor;
    }

    private void ChangeColor(DamageTextColor color)
    {
        switch (color)
        {
            case DamageTextColor.Normal:
                damageText.color = normalColor;
                break;
            case DamageTextColor.Player:
                damageText.color = playerColor;
                break;
            default:
                damageText.color = normalColor;
                break;
        }
    }
}

public enum DamageTextColor
{
    Normal,
    Player
}