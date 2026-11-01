using System.Collections.Generic;
using UnityEngine;

public class GachaController : MonoBehaviour
{
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


}
