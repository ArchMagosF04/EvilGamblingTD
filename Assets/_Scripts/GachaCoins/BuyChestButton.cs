using UnityEngine;
using Alchemy.Inspector;
using UnityEngine.UI;
using System;
using TMPro;

public class BuyChestButton : MonoBehaviour
{
    [BoxGroup("Components"), SerializeField] private SO_PlayerSavedData savedData;
    [BoxGroup("Components"), SerializeField] private Button button;
    [BoxGroup("Components"), SerializeField] private TMP_Text costText;
    [BoxGroup("Components"), SerializeField] private GachaController gachaController;

    [BoxGroup("Settings"), SerializeField] private int buttonCost;
    [BoxGroup("Settings"), SerializeField] private ProbabilityTable probabilityTable;

    private void Start()
    {
        costText.text = buttonCost.ToString();

        button.onClick.AddListener(RollGacha);
    }

    private void OnEnable()
    {
        CheckIfCanBuy();
    }

    public void CheckIfCanBuy()
    {
        if (savedData.gachaCoins < buttonCost)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }

    public void RollGacha()
    {
        gachaController.RollRarity(probabilityTable, buttonCost);
    }
}

[Serializable]
public struct ProbabilityTable
{
    public float commonWeight;
    public float rareWeight;
    public float veryRareWeight;
    public float epicWeight;
    public float legendaryWeight;
}
