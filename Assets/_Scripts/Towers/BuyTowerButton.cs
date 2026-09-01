using Alchemy.Inspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyTowerButton : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_TowerBuyData towerData;
    [BoxGroup("Components"), SerializeField] private Image towerIconImage;
    [BoxGroup("Components"), SerializeField] private Image buttonImage;
    [BoxGroup("Components"), SerializeField] private TMP_Text costText;

    private void Awake()
    {
        InitializeTowerSlot();
    }

    private void InitializeTowerSlot()
    {
        costText.text = "$" + towerData.TowerCost;

        towerIconImage.sprite = towerData.TowerButtonSprite;
    }

    public void BuyTower()
    {
        if (PlayerManager.Instance.Money >= towerData.TowerCost)
        {

        }
        else
        {

        }
    }
}
