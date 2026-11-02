using Alchemy.Inspector;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GachaController : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_PlayerSavedData savedData;
    [BoxGroup("Components"), SerializeField] private BuyChestButton[] chestButtons;
    [BoxGroup("Components"), SerializeField] private Image unlockedTowerImage;

    [SerializeField] private List<SO_TowerData> towersInTheGame = new List<SO_TowerData>();

    private List<SO_TowerData> commonTowers = new List<SO_TowerData>();
    private List<SO_TowerData> rareTowers = new List<SO_TowerData>();
    private List<SO_TowerData> veryRareTowers = new List<SO_TowerData>();
    private List<SO_TowerData> epicTowers = new List<SO_TowerData>();
    private List<SO_TowerData> legendaryTowers = new List<SO_TowerData>();

    private void Awake()
    {
        for (int i = 0; i < towersInTheGame.Count; i++)
        {
            switch (towersInTheGame[i].Rarity)
            {
                case TowerRarities.Common:
                    commonTowers.Add(towersInTheGame[i]);
                    break;
                case TowerRarities.Rare:
                    rareTowers.Add(towersInTheGame[i]);
                    break;
                case TowerRarities.VeryRare:
                    veryRareTowers.Add(towersInTheGame[i]);
                    break;
                case TowerRarities.Epic:
                    epicTowers.Add(towersInTheGame[i]);
                    break;
                case TowerRarities.Legendary:
                    legendaryTowers.Add(towersInTheGame[i]);
                    break;
            }
        }
    }

    private void OnEnable()
    {
        unlockedTowerImage.enabled = false;
    }

    public void RollRarity(ProbabilityTable table, int cost)
    {
        savedData.SpendGachaCoins(cost);

        foreach (BuyChestButton chestButton in chestButtons)
        {
            chestButton.CheckIfCanBuy();
        }

        float totalWeight = table.commonWeight + table.rareWeight + table.veryRareWeight + table.epicWeight + table.legendaryWeight;

        float randomValue = Random.Range(0, totalWeight);

        if (randomValue < table.commonWeight)
        {
            RollTower(commonTowers);
            return;
        }
        
        randomValue -= table.commonWeight;

        if (randomValue < table.rareWeight)
        {
            RollTower(rareTowers);
            return;
        }

        randomValue -= table.rareWeight;

        if (randomValue < table.veryRareWeight)
        {
            RollTower(veryRareTowers);
            return;
        }

        randomValue -= table.veryRareWeight;

        if (randomValue < table.epicWeight)
        {
            RollTower(epicTowers);
            return;
        }

        RollTower(legendaryTowers);
    }

    private void RollTower(List<SO_TowerData> rarityList)
    {
        int random = Random.Range(0, rarityList.Count);

        unlockedTowerImage.sprite = rarityList[random].InventoryImage;
        unlockedTowerImage.enabled = true;

        savedData.UnlockTower(rarityList[random].ID);
    }
}
