using Alchemy.Inspector;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyTowerButton : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_TowerData towerData;
    [BoxGroup("Components"), SerializeField] private Image towerIconImage;
    [BoxGroup("Components"), SerializeField] private Image buttonImage;
    [BoxGroup("Components"), SerializeField] private TMP_Text costText;

    [BoxGroup("Event Channels"), SerializeField] private TowerControllerEvent selectTowerEvent;

    private Sequence cantBuySequence;
    private Tween buyTween;

    [Button]
    public void InitializeTowerSlot(SO_TowerData data)
    {
        if (data == null)
        {
            gameObject.SetActive(false);
            return;
        }

        towerData = data;

        costText.text = "$" + towerData.TowerCost;

        towerIconImage.sprite = towerData.TowerButtonSprite;
    }

    public void BuyTower()
    {
        if (PlayerManager.Instance.Money >= towerData.TowerCost)
        {
            if (cantBuySequence != null && cantBuySequence.IsActive()) cantBuySequence.Kill(true);

            selectTowerEvent?.InvokeEvent(towerData);
            TowerBoughtAnim();
        }
        else
        {
            if (buyTween != null && buyTween.IsActive()) buyTween.Kill(true);

            CantBuyAnim();
        }
    }

    private void TowerBoughtAnim()
    {
        if (buyTween != null && buyTween.IsActive()) buyTween.Kill(true);

        buyTween = buttonImage.rectTransform.DOPunchScale(Vector3.one * 0.15f, 0.2f, 0, 0).SetLink(gameObject);
    }

    private void CantBuyAnim()
    {
        if (cantBuySequence != null && cantBuySequence.IsActive()) cantBuySequence.Kill(true);

        cantBuySequence = DOTween.Sequence();

        cantBuySequence.Append(buttonImage.DOColor(Color.red, 0.1f))
                       .AppendInterval(0.1f)
                       .Append(buttonImage.DOColor(Color.white, 0.15f)).SetLink(gameObject);
    }
}
