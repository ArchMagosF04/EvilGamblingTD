using Alchemy.Inspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerInventoryEntry : MonoBehaviour
{
    [SerializeField] private SO_PlayerSavedData savedData;
    [SerializeField] private SO_TowerData towerData;

    [BoxGroup("Components"), SerializeField] private Button button;
    [BoxGroup("Components"), SerializeField] private TMP_Text nameText;
    [BoxGroup("Components"), SerializeField] private Image image;

    private void OnEnable()
    {
        if (towerData == null)
        {
            button.interactable = false;
            return;
        }

        nameText.text = towerData.TowerName;
        image.sprite = towerData.InventoryImage;
        image.color = Color.white;

        if (savedData.IsTowerUnlocked(towerData.ID))
        {
            button.interactable = true;
        }
        else
        {
            button.interactable = false;
            nameText.text = "Locked";
        }
    }
}
